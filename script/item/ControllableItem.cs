using Godot;

using INTOnlineCoop.Script.Player;

namespace INTOnlineCoop.Script.Item
{
    /// <summary>
    /// Item which can receive inputs by the player
    /// </summary>
    public partial class ControllableItem : Node2D
    {
        /// <summary>
        /// Item type
        /// </summary>
        public SelectableItem Item { get; set; }

        /// <summary>
        /// Current StateMachine instance
        /// </summary>
        protected StateMachine StateMachine { get; private set; }

        /// <summary>
        /// Emitted when the item was used
        /// </summary>
        [Signal]
        public delegate void ItemUsedEventHandler(SelectableItem item, Vector2 direction);

        /// <summary>
        /// Handles the input of the player
        /// </summary>
        /// <param name="delta">Current frame delta</param>
        public virtual void HandleInput(double delta)
        {
        }

        /// <summary>
        /// Sets the current state machine instance
        /// </summary>
        /// <param name="machine">StateMachine instance</param>
        public void SetStateMachine(StateMachine machine)
        {
            StateMachine = machine;
        }
    }
}