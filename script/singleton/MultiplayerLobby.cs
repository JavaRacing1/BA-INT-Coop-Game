using Godot;
using Godot.Collections;

namespace INTOnlineCoop.Script.Singleton
{
    /// <summary>
    /// Singleton node for managing multiplayer connections
    /// </summary>
    public partial class MultiplayerLobby : Node
    {
        private const string DefaultIp = "127.0.0.1";
        private const int DefaultPort = 7070;
        private const int MaxPlayers = 2;

        /// <summary>
        /// Current MultiplayerLobby instance
        /// </summary>
        public static MultiplayerLobby Instance { get; private set; }

        /// <summary>
        /// Signal emitted after a successful player connection
        /// </summary>
        [Signal]
        public delegate void PlayerConnectedEventHandler(int peerId, Dictionary<string, string> playerInfo);

        /// <summary>
        /// Signal emitted after a player disconnects
        /// </summary>
        [Signal]
        public delegate void PlayerDisconnectedEventHandler(int peerId);

        /// <summary>
        /// Signal emitted after the server disconnected
        /// </summary>
        [Signal]
        public delegate void ServerDisconnectedEventHandler();

        /// <summary>
        /// Initializes the lobby node + starts the server
        /// </summary>
        public override void _Ready()
        {
            Instance = this;

            if (DisplayServer.GetName() == "headless" || OS.HasFeature("dedicated_server"))
            {
                //TODO: Make port configurable 
                CreateServer(DefaultPort);
            }

            Multiplayer.PeerConnected += OnPlayerConnected;
            Multiplayer.PeerDisconnected += OnPlayerDisconnected;
            Multiplayer.ConnectedToServer += OnConnectOk;
            Multiplayer.ConnectionFailed += OnConnectionFail;
            Multiplayer.ServerDisconnected += OnServerDisconnected;
        }

        public Error JoinGame(string address = "", int port = DefaultPort)
        {
            if (string.IsNullOrEmpty(address))
            {
                address = DefaultIp;
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
            Error error = serverPeer.CreateServer(port, MaxPlayers);

            if (error != Error.Ok)
            {
                GD.PrintErr("Failed to create server");
                GD.PrintErr(error.ToString());
            }

            Multiplayer.MultiplayerPeer = serverPeer;
        }

        private void OnPlayerConnected(long peerId)
        {
        }

        private void OnPlayerDisconnected(long peerId)
        {
        }

        private void OnConnectOk()
        {
        }

        private void OnConnectionFail()
        {
            Multiplayer.MultiplayerPeer = null;
        }

        private void OnServerDisconnected()
        {
        }
    }
}