using System.Collections.Generic;

using Godot;

using INTOnlineCoop.Script.Level;
using INTOnlineCoop.Script.Singleton;
using INTOnlineCoop.Script.UI.Component;
using INTOnlineCoop.Script.Util;

namespace INTOnlineCoop.Script.UI.Screen
{
    /// <summary>
    /// Screen for configuring the sandbox mode
    /// </summary>
    public partial class LobbyScreen : Control
    {
        [Export] private GeneratorSettingsContainer _generatorSettings;
        [Export] private Container _playerInformationContainer;

        private GameConfirmationDialog _quitDialog;

        /// <summary>
        /// Generate player information UI
        /// </summary>
        public override void _Ready()
        {
            RebuildPlayerInformation();
            MultiplayerLobby.Instance.PlayerDataChanged += RebuildPlayerInformation;
        }

        /// <summary>
        /// Disconnects from PlayerDataChanged signal
        /// </summary>
        public override void _ExitTree()
        {
            MultiplayerLobby.Instance.PlayerDataChanged -= RebuildPlayerInformation;
        }

        private void RebuildPlayerInformation()
        {
            if (_playerInformationContainer == null || !IsInstanceValid(_playerInformationContainer))
            {
                return;
            }

            foreach (Node child in _playerInformationContainer.GetChildren())
            {
                child.QueueFree();
            }

            Dictionary<int, PlayerData> playerDataDictionary = new();
            for (int i = 1; i <= MultiplayerLobby.MaxPlayers; i++)
            {
                playerDataDictionary.Add(i, new PlayerData());
            }

            foreach (PlayerData data in MultiplayerLobby.Instance.GetPlayerData())
            {
                if (data.PlayerNumber == -1)
                {
                    continue;
                }

                playerDataDictionary[data.PlayerNumber] = data;
            }

            PackedScene informationItemScene =
                (PackedScene)ResourceLoader.Load("res://scene/ui/component/PlayerInformationItem.tscn");
            foreach (KeyValuePair<int, PlayerData> data in playerDataDictionary)
            {
                PlayerInformationItem item = informationItemScene.Instantiate<PlayerInformationItem>();
                item.SetPlayerNumber(data.Key);
                item.SetPlayerName(data.Value.Name);
                _playerInformationContainer.AddChild(item);
            }
        }

        private void OnBackButtonPressed()
        {
            if (_quitDialog == null)
            {
                _quitDialog = new("Verbindung trennen", "Möchtest du wirklich die Verbindung zum Server trennen?");
                _quitDialog.GetOkButton().Pressed += () =>
                {
                    MultiplayerLobby.Instance.CloseConnection();
                    _ = GetTree().ChangeSceneToFile("res://scene/ui/screen/MainMenu.tscn");
                };
                AddChild(_quitDialog);
            }

            _quitDialog.Visible = true;
        }

        private void OnPlayButtonPressed()
        {
            if (_generatorSettings != null)
            {
                LevelGenerator levelGenerator = new();
                levelGenerator.SetTerrainShape(_generatorSettings.SelectedTerrainShape);
                Image image = levelGenerator.Generate(_generatorSettings.Seed);

                GameLevel level = GD.Load<PackedScene>("res://scene/level/GameLevel.tscn").Instantiate<GameLevel>();
                level.Init(image);
                GetTree().Root.AddChild(level);
                GetTree().CurrentScene = level;
                QueueFree();
            }
        }
    }
}