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
        /// <param name="playerPosition">Current position of the player</param>
        void HandleInput(InputEvent inputEvent, Vector2 playerPosition);
    }
}