using Godot;

namespace INTOnlineCoop.Script.Item
{
    /// <summary>
    /// Item which can be controlled with aim_up and aim_down
    /// </summary>
    public partial class DirectionItem : ControllableItem
    {
        [Export] private Sprite2D _crosshair;

        /// <summary>
        /// Mirrors the crosshair
        /// </summary>
        public void MirrorCrosshair()
        {
            Vector2 oldPosition = _crosshair.Position;
            _crosshair.Position = new Vector2(-oldPosition.X, oldPosition.Y);
        }
    }
}