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
        /// Handles the input for the activation item. Should be called every frame
        /// </summary>
        /// <param name="playerPosition">Current position of the player</param>
        public void HandleInput(Vector2 playerPosition)
        {
            if (!GameLevel.IsInputBlocked && Input.IsActionJustPressed("use_item"))
            {
                UseItem(playerPosition);
            }
        }

        /// <summary>
        /// Activates the item at the given position
        /// </summary>
        /// <param name="targetPosition">Target position for usage</param>
        protected abstract void UseItem(Vector2 targetPosition);
    }
}