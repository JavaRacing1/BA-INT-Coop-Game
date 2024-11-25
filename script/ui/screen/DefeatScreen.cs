using Godot;

namespace INTOnlineCoop.Script.UI.Screen
{
    /// <summary>
    /// Zeigt den Niederlangenscreen und besitzt Button zum Wechseln in Multiplayer Lobby
    /// </summary>
    public partial class DefeatScreen : Node
    {
        /// <summary>
        /// wechsel auf Multiplayer Lobby Szene
        /// </summary>
        private void OnMultiplayerLobbyButtonPressed()
        {
            _ = GetTree().ChangeSceneToFile("res://scene/ui/screen/MainMenu.tscn");
        }
    }
}


