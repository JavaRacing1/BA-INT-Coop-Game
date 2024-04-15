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
        [Export] private Camera2D _camera;
        private const int TileSize = 2;
        private const float AirThreshold = 0.3f;
        private const int RenderDistanceX = 300;
        private const int RenderDistanceY = 150;
        private const int CameraSpeed = 5;

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

        public override void _Process(double delta)
        {
            Vector2 moveVector = Vector2.Zero;
            if (Input.IsActionPressed("camera_left"))
            {
                moveVector += Vector2.Left;
            }
            if (Input.IsActionPressed("camera_up"))
            {
                moveVector += Vector2.Up;
            }
            if (Input.IsActionPressed("camera_right"))
            {
                moveVector += Vector2.Right;
            }
            if (Input.IsActionPressed("camera_down"))
            {
                moveVector += Vector2.Down;
            }
            _camera.Position += moveVector * CameraSpeed;
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