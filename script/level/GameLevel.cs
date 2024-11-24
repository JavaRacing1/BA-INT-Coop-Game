using System;

using Godot;

using INTOnlineCoop.Script.Level.Tile;
using INTOnlineCoop.Script.Player;
using INTOnlineCoop.Script.Singleton;
using INTOnlineCoop.Script.UI.Component;
using INTOnlineCoop.Script.UI.Screen;

namespace INTOnlineCoop.Script.Level
{
    /// <summary>
    /// Manager class of the level
    /// </summary>
    public partial class GameLevel : Node2D
    {
        [Export] private LevelTileManager _tileManager;
        [Export] private LevelCharacterManager _characterManager;
        [Export] private PlayerCamera _camera;
        [Export] private Node2D _characterParent;
        [Export] private CanvasLayer _userInterfaceLayer;
        [Export] private ColorRect _bottomWaterRect;
        [Export] private CollisionShape2D _waterCollisionShape;

        private Image _terrainImage;

        /// <summary>
        /// Flag variable for blocking the game inputs
        /// </summary>
        public static bool IsInputBlocked { get; set; }

        /// <summary>
        /// Initializes the level instance
        /// </summary>
        /// <param name="terrainImage">Image containing the shape of the terrain</param>
        public void Init(Image terrainImage)
        {
            _terrainImage = terrainImage;
            Vector2I tileSize = _tileManager?.GetTileSize() ?? Vector2I.Zero;
            if (_camera != null)
            {
                Vector2I terrainSize = new(terrainImage.GetWidth() * tileSize.X, terrainImage.GetHeight() * tileSize.Y);
                _camera.Init(terrainSize);
            }

            if (_bottomWaterRect != null && _waterCollisionShape != null)
            {
                Vector2 waterSize = new((terrainImage.GetWidth() * tileSize.X) + 1000, 300);
                Vector2 waterPosition = new(-500, (terrainImage.GetHeight() * tileSize.Y) - 32);
                _bottomWaterRect.Size = waterSize;
                _bottomWaterRect.Position = waterPosition;

                _waterCollisionShape.Position = waterPosition + new Vector2(waterSize.X / 2, 210);
                RectangleShape2D shape = new()
                {
                    Size = waterSize
                };
                _waterCollisionShape.SetShape(shape);
            }

            GD.Print("GameLevel initialized!");
        }

        /// <summary>
        /// Spawns a sandbox character in the level
        /// </summary>
        /// <param name="shape">Selected Terrain shape</param>
        /// <param name="characterType">Selected character type</param>
        public void SpawnSandboxCharacter(TerrainShape shape, CharacterType characterType)
        {
            if (_terrainImage == null || _tileManager == null)
            {
                return;
            }

            PlayerPositionGenerator positionGenerator = new();
            positionGenerator.Init(_terrainImage, shape.ToString(), debugMode: true);
            (double, double) unscaledSpawnPosition = positionGenerator.GetSpawnPosition(new Random().NextDouble());

            Vector2I tileSize = _tileManager?.GetTileSize() ?? Vector2I.Zero;
            Vector2 scaledSpawnPosition = new((float)unscaledSpawnPosition.Item1 * tileSize.X,
                (float)unscaledSpawnPosition.Item2 * tileSize.Y);
            PlayerCharacter character = GD.Load<PackedScene>("res://scene/player/PlayerCharacter.tscn")
                .Instantiate<PlayerCharacter>();
            character.Init(scaledSpawnPosition, characterType, 1);
            AddChild(character);
        }

        /// <summary>
        /// Called when the node enters the scene tree
        /// </summary>
        public override void _Ready()
        {
            _tileManager?.InitTileMap(_terrainImage);
            MultiplayerLobby.Instance.PlayerDisconnected += OnDisconnect;

            _camera.CameraUpdated += UpdatePlayerLabels;

            Error error = MultiplayerLobby.Instance.RpcId(1, MultiplayerLobby.MethodName.PlayerLoaded);
            if (error != Error.Ok)
            {
                GD.PrintErr("Could not send PlayerLoaded RPC: " + error);
            }
        }

        /// <summary>
        /// Disconnect signals
        /// </summary>
        public override void _ExitTree()
        {
            MultiplayerLobby.Instance.PlayerDisconnected -= OnDisconnect;
            _camera.CameraUpdated -= UpdatePlayerLabels;
        }

        /// <summary>
        /// Starts the game
        /// </summary>
        public void StartGame()
        {
            if (_characterParent == null || _characterManager == null)
            {
                return;
            }

            PlayerPositionGenerator positionGenerator = new();
            positionGenerator.Init(_terrainImage, "", debugMode: true);
            Vector2I tileSize = _tileManager?.GetTileSize() ?? Vector2I.Zero;

            _characterManager.SpawnCharacters(_characterParent, positionGenerator, tileSize);
        }

        /// <summary>
        /// Called when an InputEvent occurs
        /// </summary>
        /// <param name="event">The input event</param>
        public override void _UnhandledInput(InputEvent @event)
        {
            if (IsInputBlocked)
            {
                return;
            }

            if (@event is InputEventKey { Keycode: Key.Escape } && _userInterfaceLayer != null)
            {
                PauseDialog pauseDialog = _userInterfaceLayer.GetNodeOrNull<PauseDialog>("PauseDialog");
                if (pauseDialog == null)
                {
                    pauseDialog = GD.Load<PackedScene>("res://scene/ui/component/PauseDialog.tscn")
                        .Instantiate<PauseDialog>();
                    pauseDialog.ExitConfirmed += OnExit;
                    _userInterfaceLayer.AddChild(pauseDialog);
                }

                IsInputBlocked = true;
                pauseDialog.Visible = true;
            }
        }

        private void OnDisconnect(int peerId)
        {
            if (_characterManager.IsGameFinished)
            {
                return;
            }
            GD.Print($"{peerId} disconnected! Closing level");
            OnExit();
        }

        private void OnExit()
        {
            MultiplayerLobby.Instance.CloseConnection();
            IsInputBlocked = false;
            MainMenu menu = GD.Load<PackedScene>("res://scene/ui/screen/MainMenu.tscn").Instantiate<MainMenu>();
            GetTree().Root.AddChild(menu);
            GetTree().CurrentScene = menu;
            QueueFree();
        }

        private void UpdatePlayerLabels()
        {
            foreach (Node node in _characterParent.GetChildren())
            {
                if (node is not PlayerCharacter character)
                {
                    continue;
                }

                if (_camera.Zoom.X < 0.3)
                {
                    character.HideHealth();
                    character.DisplayCharacterIcon();
                }
                else
                {
                    character.DisplayHealth();
                    character.HideCharacterIcon();
                }
            }
        }

    }
}