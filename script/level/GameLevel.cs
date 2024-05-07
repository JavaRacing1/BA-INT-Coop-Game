using Godot;

using INTOnlineCoop.Script.Level.Tile;

namespace INTOnlineCoop.Script.Level
{
    /// <summary>
    /// Manager class of the level
    /// </summary>
    public partial class GameLevel : Node2D
    {
        [ExportGroup("Nodes")]
        [Export] private LevelTileManager _tileManager;
        [Export] private Camera2D _camera;

        [ExportGroup("CameraSettings")]
        // Maximum pixel offsets of the camera to the terrain, used for calculating the horizontal camera limits
        [Export(PropertyHint.Range, "0,10000,")] private int _cameraLimitOffsetX = 200;
        [Export(PropertyHint.Range, "0,10000,")] private int _cameraLimitOffsetY = 80;

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
                _camera.LimitLeft = -_cameraLimitOffsetX;
                _camera.LimitTop = -_cameraLimitOffsetY * 2;
                _camera.LimitRight = (terrainImage.GetWidth() * tileSize.X) + _cameraLimitOffsetX;
                _camera.LimitBottom = (terrainImage.GetHeight() * tileSize.Y) + _cameraLimitOffsetY;

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