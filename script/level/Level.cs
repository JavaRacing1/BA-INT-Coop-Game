using Godot;

using INTOnlineCoop.Script.Test;

namespace INTOnlineCoop.Script.Level
{
    /// <summary>
    /// Main level scene
    /// </summary>
    public partial class Level : Node2D
    {
        [Export] private FastNoiseLite _levelNoise;
        [Export] private PackedScene _levelTile;
        private const int TileSize = 10;
        private const float AirThreshold = 0.3f;
        private const int RenderDistanceX = 64;
        private const int RenderDistanceY = 36;

        public override void _Ready()
        {
            for (int x = 0; x < RenderDistanceX; x++)
            {
                for (int y = 0; y < RenderDistanceY; y++)
                {
                    GenerateTerrainTile(x, y);
                }
            }
        }

        private void GenerateTerrainTile(int x, int y)
        {
            LevelTile tile = _levelTile.Instantiate<LevelTile>();
            tile.SetTileType(GetValueForTile(x, y));
            tile.Position = new Vector2(x, y) * TileSize;
            AddChild(tile);
        }

        private LevelTileType GetValueForTile(int x, int y)
        {
            float noise = _levelNoise.GetNoise2D(x, y);
            return noise >= AirThreshold ? LevelTileType.LAND : LevelTileType.AIR;
        }
    }
}