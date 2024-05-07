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
        [Export] private Camera2D _camera;

        /// <summary>
        /// Maximum pixel x-offset of the camera to the terrain, used for calculating the horizontal camera limits
        /// </summary>
        private const int CameraLimitOffsetX = 200;

        /// <summary>
        /// Maximum pixel y-offset of the camera to the terrain, used for calculating the vertical camera limits
        /// </summary>
        private const int CameraLimitOffsetY = 50;

        private Image _terrainImage;

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
                _camera.LimitLeft = -CameraLimitOffsetX;
                _camera.LimitTop = -CameraLimitOffsetY * 2;
                _camera.LimitRight = (terrainImage.GetWidth() * tileSize.X) + CameraLimitOffsetX;
                _camera.LimitBottom = (terrainImage.GetHeight() * tileSize.Y) + CameraLimitOffsetY;

                _camera.Position = new Vector2(terrainImage.GetWidth() * tileSize.X / 2f,
                    terrainImage.GetHeight() * tileSize.Y / 2f);
            }

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