using Godot;

using INTOnlineCoop.Script.Level;

namespace INTOnlineCoop.Script.Item
{
    /// <summary>
    /// Item without any aiming
    /// </summary>
    public abstract class ActivationItem : IItem
    {
        /// <summary>
        /// Handles the input for the activation item
        /// </summary>
        /// <param name="inputEvent">Input event</param>
        /// <param name="playerPosition">Current position of the player</param>
        public void HandleInput(InputEvent inputEvent, Vector2 playerPosition)
        {
            if (!GameLevel.IsInputBlocked && inputEvent.IsAction("use_item"))
            {
                UseItem(playerPosition);
            }
        }

        /// <summary>
        /// Activates the item
        /// </summary>
        protected abstract void UseItem(Vector2 targetPosition);
    }
}