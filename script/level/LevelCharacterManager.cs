using System;
using System.Collections.Generic;
using System.Linq;

using Godot;

using INTOnlineCoop.Script.Item;
using INTOnlineCoop.Script.Player;
using INTOnlineCoop.Script.Singleton;

namespace INTOnlineCoop.Script.Level
{
    /// <summary>
    /// Enum containing all possible winner states
    /// </summary>
    public enum Winner
    {
        /// <summary>No person has won yet</summary>
        None,

        /// <summary>First player has won</summary>
        PlayerOne,

        /// <summary>Second player has won</summary>
        PlayerTwo
    }

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
        [Export] private ColorRect _bottomWaterRect;
        [Export] private CollisionShape2D _waterCollisionShape;

        [Export(PropertyHint.Range, "0,50,")] private int _waterRisingMinRound = 16;

        [Export(PropertyHint.Range, "0,1000,")]
        private int _waterRisingAmount = 48;

        private Node _characterParent;
        private int _currentCharacterIndex;
        private long _currentPlayerPeer;
        private int _currentRoundNumber;

        /// <summary>
        /// True if the game has ended
        /// </summary>
        public bool IsGameFinished { get; private set; }

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
            if (Multiplayer.HasMultiplayerPeer() && Multiplayer.IsServer())
            {
                _roundTimer.Timeout -= NextCharacter;
            }

            if (_characterParent == null)
            {
                return;
            }

            foreach (Node node in _characterParent.GetChildren())
            {
                if (node is not PlayerCharacter character)
                {
                    continue;
                }

                character.PlayerDied -= OnPlayerDeath;
            }
        }

        /// <summary>
        /// Spawns all player characters
        /// </summary>
        public void SpawnCharacters(Node parentNode, PlayerPositionGenerator generator, Vector2I tileSize)
        {
            _characterParent = parentNode;
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
                    character.PlayerDied += OnPlayerDeath;
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
            _ = _characterOrder[_currentCharacterIndex].Rpc(PlayerCharacter.MethodName.SetItem, SelectableItem.None.Name);
            int characterIndex = _currentCharacterIndex;
            int loopIndex = 0;
            PlayerCharacter nextCharacter = null;
            while (loopIndex < _characterOrder.Length + 1)
            {
                loopIndex++;
                characterIndex = (characterIndex + 1) % _characterOrder.Length;
                PlayerCharacter character = _characterOrder[characterIndex];
                if ((_currentPlayerPeer != 0 && character.PeerId == _currentPlayerPeer) || (character.Health <= 0))
                {
                    continue;
                }

                nextCharacter = character;
                _currentCharacterIndex = characterIndex;
                _currentPlayerPeer = character.PeerId;
                break;
            }

            if (nextCharacter == null || _roundTimer == null)
            {
                return;
            }

            _currentRoundNumber++;

            nextCharacter.IsBlocked = false;
            Vector2 newCharacterPos = nextCharacter.Position;
            long peerId = nextCharacter.PeerId;

            Error error = _userInterface.Rpc(GameLevelUserInterface.MethodName.HideTimerLabel);
            if (error != Error.Ok)
            {
                GD.PrintErr("Error while hiding timer: " + error);
            }

            SceneTreeTimer timer = GetTree().CreateTimer(5);
            timer.Timeout += () =>
            {
                if (!IsInstanceValid(this))
                {
                    return;
                }
                error = Rpc(MethodName.StartRound, newCharacterPos, peerId,
                    _currentRoundNumber > _waterRisingMinRound);
                if (error != Error.Ok)
                {
                    GD.PrintErr("Error while resetting timer: " + error);
                }
            };
        }

        [Rpc(CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
        private void StartRound(Vector2 characterPosition, long peerId, bool isWaterRising)
        {
            _roundTimer.WaitTime = 65;
            _roundTimer.OneShot = true;
            _roundTimer.Start();

            if (_camera != null)
            {
                _camera.ChangeCameraZoom(1);
                _camera.MoveCamera(characterPosition);
            }

            if (isWaterRising)
            {
                RiseWater();
            }

            if (_userInterface != null)
            {
                PlayerData data = MultiplayerLobby.Instance.GetPlayerData(peerId);
                _userInterface.DisplayTurnNotification(data.Name, data.PlayerNumber, isWaterRising);
                _userInterface.DisplayWeapons(data.PlayerNumber);
            }
        }

        private void OnPlayerDeath(PlayerCharacter character)
        {
            if (!Multiplayer.IsServer())
            {
                return;
            }
            if (character == _characterOrder[_currentCharacterIndex])
            {
                NextCharacter();
            }

            character.StateMachine.TransitionTo(AvailableState.Dead);

            Winner potentialWinner = GetWinner();
            if (potentialWinner != Winner.None)
            {
                IsGameFinished = true;
                _ = Rpc(MethodName.EndGame, potentialWinner.ToString());
            }
        }

        private Winner GetWinner()
        {
            int[] playerDeadAmount = { 0, 0 };
            foreach (PlayerCharacter orderChar in _characterOrder)
            {
                if (orderChar.Health > 0)
                {
                    continue;
                }

                int playerNumber = MultiplayerLobby.Instance.GetPlayerData(orderChar.PeerId).PlayerNumber;
                playerDeadAmount[playerNumber - 1]++;
            }

            return playerDeadAmount[0] == 4
                ? Winner.PlayerTwo
                : (playerDeadAmount[1] == 4 ? Winner.PlayerOne : Winner.None);
        }

        [Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
        private void EndGame(string winnerString)
        {
            IsGameFinished = true;
            bool parseOk = Enum.TryParse(winnerString, true, out Winner winner);
            if (!parseOk)
            {
                GD.PrintErr("Received invalid winner string: " + winnerString);
                return;
            }

            int playerNumber = MultiplayerLobby.Instance.GetPlayerData(Multiplayer.GetUniqueId()).PlayerNumber;
            Node screen = playerNumber == (int)winner
                ? GD.Load<PackedScene>("res://scene/ui/screen/VictoryScreen.tscn").Instantiate()
                : GD.Load<PackedScene>("res://scene/ui/screen/DefeatScreen.tscn").Instantiate();

            MultiplayerLobby.Instance.CloseConnection();
            GetTree().Root.AddChild(screen);
            GetTree().CurrentScene = screen;
            GetParent().QueueFree();
        }

        private void OnWaterEntered(Node2D body)
        {
            if (body is not PlayerCharacter character || !Multiplayer.IsServer())
            {
                return;
            }

            Error error = character.Rpc(PlayerCharacter.MethodName.Damage, 1000);
            if (error != Error.Ok)
            {
                GD.PrintErr("Error during PlayerDied RPC: " + error);
            }
        }

        private void RiseWater()
        {
            _bottomWaterRect.Position -= new Vector2(0, _waterRisingAmount);
            _bottomWaterRect.Size += new Vector2(_waterRisingAmount, _waterRisingAmount);

            RectangleShape2D oldWaterShape = (RectangleShape2D)_waterCollisionShape.Shape;
            RectangleShape2D newWaterShape = new()
            {
                Size = new Vector2(oldWaterShape.Size.X, oldWaterShape.Size.Y + _waterRisingAmount)
            };
            _waterCollisionShape.Position -= new Vector2(0, _waterRisingAmount * 0.4f);
            _waterCollisionShape.SetShape(newWaterShape);
        }
    }
}