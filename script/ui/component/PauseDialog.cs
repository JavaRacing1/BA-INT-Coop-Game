using Godot;

using INTOnlineCoop.Script.Level;
using INTOnlineCoop.Script.UI.Screen;

namespace INTOnlineCoop.Script.UI.Component
{
    /// <summary>
    /// Window when a player presses ESC in a level
    /// </summary>
    public partial class PauseDialog : Control
    {
        private SettingsWindow _settingsWindow;

        private void OnResumeButtonPressed()
        {
            Visible = false;
            GameLevel.IsInputBlocked = false;
        }

        private void OnSettingsButtonPressed()
        {
            if (_settingsWindow == null)
            {
                _settingsWindow = GD.Load<PackedScene>("res://scene/ui/screen/SettingsWindow.tscn")
                    .Instantiate<SettingsWindow>();
                AddChild(_settingsWindow);
            }

            _settingsWindow.Layer = 2;
            _settingsWindow.Visible = true;
        }
    }
}