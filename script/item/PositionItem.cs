using Godot;

using INTOnlineCoop.Script.Level;

namespace INTOnlineCoop.Script.Item
{
    /// <summary>
    /// Item for which the exact target position needs to be specified
    /// </summary>
    public abstract class PositionItem : IItem
    {
        private Vector2 _initialPosition = Vector2.Zero;

        /// <summary>
        /// Handles the input for the position item
        /// </summary>
        /// <param name="inputEvent">Input event</param>
        /// <param name="playerPosition">Current position of the player</param>
        public void HandleInput(InputEvent inputEvent, Vector2 playerPosition)
        {
            if (GameLevel.IsInputBlocked)
            {
                return;
            }

            if (_initialPosition == Vector2.Zero)
            {
                _initialPosition = playerPosition;
            }

            if (inputEvent.IsAction("use_item"))
            {
                UseItem(_initialPosition);
            }

            //TODO: Add position movement (input actions + mouse)
        }

        /// <summary>
        /// Activates the item at the given position
        /// </summary>
        /// <param name="targetPosition">Target position for usage</param>
        protected abstract void UseItem(Vector2 targetPosition);
    }
}