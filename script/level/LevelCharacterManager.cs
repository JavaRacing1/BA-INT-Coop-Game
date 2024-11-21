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

        private int _currentCharacterIndex;

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
        }

        /// <summary>
        /// Ends the current round and starts the round of the next player
        /// </summary>
        public void NextCharacter()
        {
            _characterOrder[_currentCharacterIndex].IsBlocked = true;
            _currentCharacterIndex = (_currentCharacterIndex + 1) % _characterOrder.Length;
            _characterOrder[_currentCharacterIndex].IsBlocked = false;
        }
    }
}