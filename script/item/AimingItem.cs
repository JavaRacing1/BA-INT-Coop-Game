using Godot;

using INTOnlineCoop.Script.Level;

namespace INTOnlineCoop.Script.Item
{
    /// <summary>
    /// Item with aiming -> Only direction is required
    /// </summary>
    public abstract class AimingItem : IItem
    {
        private int _rotation;

        /// <summary>
        /// Handles the input for the aiming item
        /// </summary>
        /// <param name="inputEvent">Input event</param>
        /// <param name="playerPosition">Current position of the player</param>
        public void HandleInput(InputEvent inputEvent, Vector2 playerPosition)
        {
            if (GameLevel.IsInputBlocked)
            {
                return;
            }

            if (inputEvent.IsAction("use_item"))
            {
                UseItem(playerPosition, _rotation);
            }

            //TODO: Add rotation change
            _rotation = _rotation;
        }

        /// <summary>
        /// Activates the item at the given position with the given rotation
        /// </summary>
        /// <param name="targetPosition">Target position for usage</param>
        /// <param name="rotation">Rotation of the aiming (up == 0, right == 90, etc.)</param>
        protected abstract void UseItem(Vector2 targetPosition, int rotation);
    }
}