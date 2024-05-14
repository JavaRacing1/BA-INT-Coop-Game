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
        public void HandleInput(InputEvent inputEvent)
        {
            if (!GameLevel.IsInputBlocked && inputEvent.IsAction("use_item"))
            {
                UseItem();
            }
        }

        /// <summary>
        /// Activates the item
        /// </summary>
        protected abstract void UseItem();
    }
}