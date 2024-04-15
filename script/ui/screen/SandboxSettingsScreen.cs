using Godot;

namespace INTOnlineCoop.Script.UI.Screen
{
    /// <summary>
    /// Screen for configuring the sandbox mode
    /// </summary>
    public partial class SandboxSettingsScreen : Control
    {
        private void OnBackButtonPressed()
        {
            GetTree().ChangeSceneToFile("res://scene/ui/screen/MainMenu.tscn");
        }

        private void OnPlayButtonPressed()
        {
            GetTree().ChangeSceneToFile("res://scene/level/Level.tscn");
        }
    }
}
