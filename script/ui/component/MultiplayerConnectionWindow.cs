using Godot;

namespace INTOnlineCoop.Script.UI.Component
{
    /// <summary>
    /// Window for server connection settings
    /// </summary>
    public partial class MultiplayerConnectionWindow : Window
    {
        private void OnCloseRequested()
        {
            Visible = false;
        }
    }
}

