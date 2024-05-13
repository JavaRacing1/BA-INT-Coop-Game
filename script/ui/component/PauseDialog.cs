using Godot;

using INTOnlineCoop.Script.Level;

namespace INTOnlineCoop.Script.UI.Component
{
    /// <summary>
    /// Window when a player presses ESC in a level
    /// </summary>
    public partial class PauseDialog : Control
    {
        private void OnResumeButtonPressed()
        {
            Visible = false;
            GameLevel.IsInputBlocked = false;
        }
    }
}