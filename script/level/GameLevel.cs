using Godot;

using INTOnlineCoop.Script.Level.Tile;
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
        [Export] private PlayerCamera _camera;
        [Export] private CanvasLayer _userInterfaceLayer;

        private Image _terrainImage;

        /// <summary>
        /// Flag variable for blocking the game inputs
        /// </summary>
        public static bool IsInputBlocked
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes the level instance
        /// </summary>
        /// <param name="terrainImage">Image containing the shape of the terrain</param>
        public void Init(Image terrainImage)
        {
            _terrainImage = terrainImage;
            if (_camera != null)
            {
                Vector2I tileSize = _tileManager?.GetTileSize() ?? Vector2I.Zero;
                Vector2I terrainSize = new(terrainImage.GetWidth() * tileSize.X, terrainImage.GetHeight() * tileSize.Y);
                _camera.Init(terrainSize);
            }

            GD.Print("GameLevel initialized!");
        }

        /// <summary>
        /// Called when the node enters the scene tree
        /// </summary>
        public override void _Ready()
        {
            _tileManager?.InitTileMap(_terrainImage);
            MultiplayerLobby.Instance.PlayerDisconnected += OnDisconnect;
        }

        /// <summary>
        /// Disconnect signals
        /// </summary>
        public override void _ExitTree()
        {
            MultiplayerLobby.Instance.PlayerDisconnected -= OnDisconnect;
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
    }
}
