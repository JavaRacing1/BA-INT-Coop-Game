using Godot;

namespace INTOnlineCoop.Script.UI.Screen
{
    /// <summary>
    /// Zeigt den Siegscreen und besitzt Button zum Wechseln in Multiplayer Lobby
    /// </summary>
    public partial class VictoryScreen : Node
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