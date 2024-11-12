using Godot;

namespace INTOnlineCoop.Script.Singleton
{
    /// <summary>
    /// Singleton node for managing multiplayer connections
    /// </summary>
    public partial class MultiplayerLobby : Node
    {
        private const string DEFAULT_IP = "127.0.0.1";
        private const int DEFAULT_PORT = 7070;
        private const int MAX_PLAYERS = 2;

        /// <summary>
        /// Current MultiplayerLobby instance
        /// </summary>
        public static MultiplayerLobby Instance { get; private set; }

        /// <summary>
        /// Initializes the lobby node + starts the server
        /// </summary>
        public override void _Ready()
        {
            Instance = this;

            if (DisplayServer.GetName() == "headless" || OS.HasFeature("dedicated_server"))
            {
                //TODO: Make port configurable 
                CreateServer(DEFAULT_PORT);
            }
        }

        public Error JoinGame(string address = "", int port = DEFAULT_PORT)
        {
            if (string.IsNullOrEmpty(address))
            {
                address = DEFAULT_IP;
            }

            ENetMultiplayerPeer clientPeer = new();
            Error error = clientPeer.CreateClient(address, port);
            if (error != Error.Ok)
            {
                GD.PrintErr("Failed to create client");
                GD.PrintErr(error.ToString());
                return error;
            }

            Multiplayer.MultiplayerPeer = clientPeer;
            return Error.Ok;
        }

        private void CreateServer(int port)
        {
            ENetMultiplayerPeer serverPeer = new();
            Error error = serverPeer.CreateServer(port, MAX_PLAYERS);

            if (error != Error.Ok)
            {
                GD.PrintErr("Failed to create server");
                GD.PrintErr(error.ToString());
            }

            Multiplayer.MultiplayerPeer = serverPeer;
        }
    }
}

