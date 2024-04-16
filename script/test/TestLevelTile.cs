using Godot;

namespace INTOnlineCoop.Script.Test
{
    public enum LevelTileType
    {
        AIR,
        LAND
    }
    public partial class TestLevelTile : Node2D
    {
        [Export] private LevelTileType _tileType;
        [Export] private Sprite2D _sprite;

        public override void _Ready()
        {
            _sprite.Modulate = _tileType == LevelTileType.AIR ? new Color(0, 1, 1) : Colors.White;
        }

        public void SetTileType(LevelTileType tileType)
        {
            _tileType = tileType;
        }
    }
}
