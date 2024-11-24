using Godot;

namespace INTOnlineCoop.Script.Item
{
    /// <summary>
    /// Item which can be controlled with aim_up and aim_down
    /// </summary>
    public partial class DirectionItem : ControllableItem
    {
        [Export] private Sprite2D _crosshair;

        private const float RotationAmount = 1f / 100;
        private bool _blockInput;

        /// <summary>
        /// Handles the direction inputs
        /// </summary>
        /// <param name="delta"></param>
        public override void HandleInput(double delta)
        {
            if (_blockInput)
            {
                return;
            }

            if (Input.IsActionPressed("aim_up"))
            {
                StateMachine.ItemRotation += Mathf.IsEqualApprox(Scale.X, 1) ? -RotationAmount : RotationAmount;
                return;
            }

            if (Input.IsActionPressed("aim_down"))
            {
                StateMachine.ItemRotation += Mathf.IsEqualApprox(Scale.X, 1) ? RotationAmount : -RotationAmount;
                return;
            }

            StateMachine.ItemRotation = 0;
            if (Input.IsActionJustPressed("use_item"))
            {
            }
        }
    }
}