using System;

using Godot;

using INTOnlineCoop.Script.Singleton;

namespace INTOnlineCoop.Script.UI.Component
{
    /// <summary>
    /// Window for server connection settings
    /// </summary>
    public partial class MultiplayerConnectionWindow : Window
    {
        [Export] private LineEdit _usernameInput;
        [Export] private LineEdit _serverAddressInput;
        [Export] private LineEdit _portInput;
        [Export] private Button _connectionButton;
        [Export] private Label _errorLabel;

        private void OnCloseRequested()
        {
            Visible = false;
        }

        private void OnConnectButtonPressed()
        {
            if (_usernameInput == null || _serverAddressInput == null || _portInput == null)
            {
                return;
            }
            DisplayError("");

            string username = _usernameInput.Text;
            if (username == "")
            {
                username = GenerateUsername();
            }

            MultiplayerLobby.Instance.CreatePlayerData(username);

            string serverAddress = _serverAddressInput.Text;
            string portString = _portInput.Text;

            Error connectionError;
            if (portString != "")
            {
                try
                {
                    int port = Convert.ToInt32(portString);
                    connectionError = MultiplayerLobby.Instance.JoinGame(DisplayTimeoutError, serverAddress, port);
                }
                catch (Exception e)
                {
                    GD.PrintErr("Port is not a valid number: " + e.Message);
                    DisplayError("Fehler: Port ist keine gültige Zahl!");
                    return;
                }
            }
            else
            {
                connectionError = MultiplayerLobby.Instance.JoinGame(DisplayTimeoutError, serverAddress);
            }

            if (connectionError != Error.Ok)
            {
                GD.PrintErr("Server connection failed: " + connectionError);
                DisplayError("Fehler beim Verbinden: " + connectionError);
            }
            else if (_connectionButton != null)
            {
                _connectionButton.Disabled = true;
            }
        }

        private string DisplayTimeoutError()
        {
            DisplayError("Fehler beim Verbinden: Timeout");
            if (_connectionButton != null)
            {
                _connectionButton.Disabled = false;
            }

            return "Timeout";
        }

        private void DisplayError(string message)
        {
            if (_errorLabel != null)
            {
                _errorLabel.Text = message;
            }
        }

        private static string GenerateUsername()
        {
            string username = "";
            Random random = new();
            for (int i = 0; i < 6; i++)
            {
                username += random.Next(10).ToString();
            }

            return username;
        }
    }
}