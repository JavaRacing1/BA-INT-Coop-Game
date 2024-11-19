using System;

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
        /// <summary>
        /// Spawns all player characters
        /// </summary>
        public void SpawnCharacters(Node parentNode, PlayerPositionGenerator generator, Vector2I tileSize)
        {
            PackedScene scene = GD.Load<PackedScene>("res://scene/player/PlayerCharacter.tscn");
            foreach (long peerId in MultiplayerLobby.Instance.GetPeerIds())
            {
                PlayerData data = MultiplayerLobby.Instance.GetPlayerData(peerId);
                foreach (CharacterType type in data.Characters)
                {
                    (double, double) unscaledSpawnPosition = generator.GetSpawnPosition(new Random().NextDouble());
                    Vector2 scaledSpawnPosition = new((float)unscaledSpawnPosition.Item1 * tileSize.X,
                        (float)unscaledSpawnPosition.Item2 * tileSize.Y);

                    PlayerCharacter character = scene.Instantiate<PlayerCharacter>();
                    character.Init(scaledSpawnPosition, type, peerId);
                    parentNode.AddChild(character);
                }
            }
        }
    }
}