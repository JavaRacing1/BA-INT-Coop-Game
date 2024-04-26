using Godot;

using INTOnlineCoop.Script.Level;

namespace INTOnlineCoop.Script.UI.Screen
{
    /// <summary>
    /// Screen for configuring the sandbox mode
    /// </summary>
    public partial class SandboxSettingsScreen : Control
    {
        private void OnBackButtonPressed()
        {
            _ = GetTree().ChangeSceneToFile("res://scene/ui/screen/MainMenu.tscn");
        }

        private void OnPlayButtonPressed()
        {
            _ = new LevelGenerator();
            GameLevel level = GD.Load<PackedScene>("res://scene/level/GameLevel.tscn").Instantiate<GameLevel>();
            GetTree().Root.AddChild(level);
            QueueFree();
        }
    }
}
