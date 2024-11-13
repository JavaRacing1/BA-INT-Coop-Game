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

        /// <summary>
        /// Generate player information UI
        /// </summary>
        public override void _Ready()
        {
            RebuildPlayerInformation();
            MultiplayerLobby.Instance.PlayerDataChanged += RebuildPlayerInformation;
        }

        private void RebuildPlayerInformation()
        {
            if (_playerInformationContainer == null)
            {
                return;
            }
            foreach (Node child in _playerInformationContainer.GetChildren())
            {
                child.QueueFree();
            }

            PackedScene informationItemScene = (PackedScene)ResourceLoader.Load("res://scene/ui/component/PlayerInformationItem.tscn");
            foreach (PlayerData data in MultiplayerLobby.Instance.GetPlayerData())
            {
                if (data.PlayerNumber == -1)
                {
                    continue;
                }
                PlayerInformationItem item = informationItemScene.Instantiate<PlayerInformationItem>();
                item.SetPlayerNumber(data.PlayerNumber);
                item.SetPlayerName(data.Name);
                _playerInformationContainer.AddChild(item);
            }
        }

        private void OnBackButtonPressed()
        {
            _ = GetTree().ChangeSceneToFile("res://scene/ui/screen/MainMenu.tscn");
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