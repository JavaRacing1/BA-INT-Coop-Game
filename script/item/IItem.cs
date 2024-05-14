using Godot;

namespace INTOnlineCoop.Script.Item
{
    /// <summary>
    /// Interface for a player item
    /// </summary>
    public interface IItem
    {
        /// <summary>
        /// Handles the input for the item
        /// </summary>
        /// <param name="inputEvent">Input event</param>
        void HandleInput(InputEvent inputEvent);
    }
}