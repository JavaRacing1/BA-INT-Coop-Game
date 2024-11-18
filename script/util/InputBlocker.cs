using Godot;

using INTOnlineCoop.Script.Level;
using INTOnlineCoop.Script.Player;

namespace INTOnlineCoop.Script.Util
{
    /// <summary>
    /// Helper class for blocking inputs
    /// </summary>
    public static class InputBlocker
    {
        /// <summary>
        /// Returns true when the user has started pressing the action event in the current frame or physics tick
        /// </summary>
        /// <param name="character">Current active character instance</param>
        /// <param name="action">Name of the action</param>
        /// <returns>True if the action was just pressed</returns>
        public static bool IsActionJustPressed(PlayerCharacter character, string action)
        {
            return !GameLevel.IsInputBlocked && !character.IsBlocked && Input.IsActionJustPressed(action);
        }

        /// <summary>
        /// Returns true if you are pressing the action event
        /// </summary>
        /// <param name="character">Current active character instance</param>
        /// <param name="action">Name of the action</param>
        /// <returns>True if the action was just pressed</returns>
        public static bool IsActionPressed(PlayerCharacter character, string action)
        {
            return !GameLevel.IsInputBlocked && !character.IsBlocked && Input.IsActionPressed(action);
        }

        /// <summary>
        /// Get axis input by specifying two actions, one negative and one positive.
        /// </summary>
        /// <param name="character">Current active character instance</param>
        /// <param name="negativeAction">Action which results in a negative direction</param>
        /// <param name="positiveAction">Action which results in a positive direction</param>
        /// <returns></returns>
        public static float GetAxis(PlayerCharacter character, string negativeAction, string positiveAction)
        {
            return (GameLevel.IsInputBlocked || character.IsBlocked)
                ? 0.0F
                : Input.GetAxis(negativeAction, positiveAction);
        }
    }
}