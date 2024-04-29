using Godot;

using INTOnlineCoop.Script.Level.Tile;

namespace INTOnlineCoop.Script.Level
{
    /// <summary>
    /// Manager class of the level
    /// </summary>
    public partial class GameLevel : Node2D
    {
        [Export] private LevelTileManager _tileManager;

        private Image _terrainImage;

        /// <summary>
        /// Initializes the level instance
        /// </summary>
        /// <param name="terrainImage">Image containing the shape of the terrain</param>
        public void Init(Image terrainImage)
        {
            _terrainImage = terrainImage;
            GD.Print("GameLevel initialized!");
        }

        /// <summary>
        /// Called when the node enters the scene tree
        /// </summary>
        public override void _Ready()
        {
            if (_tileManager != null)
            {
                _tileManager.InitTileMap(_terrainImage);
            }
        }
    }
}