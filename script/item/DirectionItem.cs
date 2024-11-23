using Godot;

namespace INTOnlineCoop.Script.Item
{
    /// <summary>
    /// Item which can be controlled with aim_up and aim_down
    /// </summary>
    public partial class DirectionItem : ControllableItem
    {
        [Export] private Sprite2D _crosshair;
    }
}