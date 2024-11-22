using System;
using System.Collections.Generic;
using System.Linq;

using Godot;

using INTOnlineCoop.Script.Player;
using INTOnlineCoop.Script.Singleton;

namespace INTOnlineCoop.Script.Level
{
    /// <summary>
    /// Manages all characters and their turns
    /// </summary>
    public partial class LevelCharacterManager : Node2D
    {
        private static readonly Random ShuffleRandom = new();
        private readonly PlayerCharacter[] _characterOrder = new PlayerCharacter[MultiplayerLobby.MaxPlayers * 4];

        [Export] private Timer _roundTimer;
        [Export] private PlayerCamera _camera;
        [Export] private GameLevelUserInterface _userInterface;

        private int _currentCharacterIndex;


        /// <summary>
        /// Add round change on timer timeout
        /// </summary>
        public override void _Ready()
        {
            if (Multiplayer.IsServer())
            {
                _roundTimer.Timeout += NextCharacter;
            }
        }

        /// <summary>
        /// Remove timer signal
        /// </summary>
        public override void _ExitTree()
        {
            if (Multiplayer.IsServer())
            {
                _roundTimer.Timeout -= NextCharacter;
            }
        }

        /// <summary>
        /// Spawns all player characters
        /// </summary>
        public void SpawnCharacters(Node parentNode, PlayerPositionGenerator generator, Vector2I tileSize)
        {
            PackedScene scene = GD.Load<PackedScene>("res://scene/player/PlayerCharacter.tscn");
            double spawnSeed = new Random().NextDouble();

            int peerIndex = -1;
            int peerAmount = MultiplayerLobby.MaxPlayers;
            foreach (long peerId in MultiplayerLobby.Instance.GetPeerIds())
            {
                peerIndex++;
                List<PlayerCharacter> characters = new(4);

                PlayerData data = MultiplayerLobby.Instance.GetPlayerData(peerId);
                foreach (CharacterType type in data.Characters)
                {
                    (double, double) unscaledSpawnPosition = generator.GetSpawnPosition(spawnSeed);
                    Vector2 scaledSpawnPosition = new((float)unscaledSpawnPosition.Item1 * tileSize.X,
                        (float)unscaledSpawnPosition.Item2 * tileSize.Y);

                    PlayerCharacter character = scene.Instantiate<PlayerCharacter>();
                    character.Init(scaledSpawnPosition, type, peerId);
                    character.IsBlocked = true;
                    parentNode.AddChild(character, true);

                    characters.Add(character);
                }

                List<PlayerCharacter> shuffledCharacters = characters.OrderBy(_ => ShuffleRandom.Next()).ToList();
                for (int i = 0; i < shuffledCharacters.Count; i++)
                {
                    _characterOrder[peerIndex + (i * peerAmount)] = shuffledCharacters[i];
                }
            }

            NextCharacter();
        }

        /// <summary>
        /// Ends the current round and starts the round of the next player
        /// </summary>
        public void NextCharacter()
        {
            _characterOrder[_currentCharacterIndex].IsBlocked = true;
            _currentCharacterIndex = (_currentCharacterIndex + 1) % _characterOrder.Length;

            PlayerCharacter character = _characterOrder[_currentCharacterIndex];
            character.IsBlocked = false;
            Vector2 newCharacterPos = character.Position;
            long peerId = character.PeerId;

            if (_roundTimer == null)
            {
                return;
            }

            Error error = _userInterface.Rpc(GameLevelUserInterface.MethodName.HideTimerLabel);
            if (error != Error.Ok)
            {
                GD.PrintErr("Error while hiding timer: " + error);
            }

            SceneTreeTimer timer = GetTree().CreateTimer(5);
            timer.Timeout += () =>
            {
                error = Rpc(MethodName.StartRound, newCharacterPos, peerId);
                if (error != Error.Ok)
                {
                    GD.PrintErr("Error while resetting timer: " + error);
                }
            };
        }

        [Rpc(CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
        private void StartRound(Vector2 characterPosition, long peerId)
        {
            _roundTimer.WaitTime = 65;
            _roundTimer.OneShot = true;
            _roundTimer.Start();

            if (_camera != null)
            {
                _camera.Position = characterPosition;
                _camera.Zoom = Vector2.One;
            }

            if (_userInterface != null)
            {
                PlayerData data = MultiplayerLobby.Instance.GetPlayerData(peerId);
                _userInterface.DisplayTurnNotification(data.Name, data.PlayerNumber);
            }
        }
    }
}