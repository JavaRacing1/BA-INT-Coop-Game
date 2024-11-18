using Godot;

using INTOnlineCoop.Script.Level;
using INTOnlineCoop.Script.Player;
using INTOnlineCoop.Script.UI.Component;

namespace INTOnlineCoop.Script.UI.Screen
{
    /// <summary>
    /// Screen for configuring the sandbox mode
    /// </summary>
    public partial class SandboxSettingsScreen : Control
    {
        [Export] private GeneratorSettingsContainer _generatorSettings;
        [Export] private GridContainer _characterGrid;
        [Export] private Button _playButton;

        private CharacterType _selectedCharacter;

        /// <summary>
        /// Fills the character grid with all available characters
        /// </summary>
        public override void _Ready()
        {
            if (_characterGrid == null || _playButton == null)
            {
                return;
            }

            _playButton.Disabled = true;

            PackedScene itemScene = GD.Load<PackedScene>("res://scene/ui/component/SandboxSelectionItem.tscn");
            foreach (CharacterType type in CharacterType.Values)
            {
                SandboxSelectionItem item = itemScene.Instantiate<SandboxSelectionItem>();
                item.SetCharacter(type);
                item.SelectedCharacterChanged += OnSelectedCharacterChanged;
                _characterGrid.AddChild(item);
                if (type == CharacterType.Gaige || type == CharacterType.Nisha || type == CharacterType.Zero)
                {
                    continue;
                }

                Control spacer = new()
                {
                    SizeFlagsHorizontal = SizeFlags.ExpandFill,
                    SizeFlagsVertical = SizeFlags.ExpandFill
                };
                _characterGrid.AddChild(spacer);
            }

            UpdateButtons();
        }

        private void OnSelectedCharacterChanged(CharacterType type)
        {
            _playButton.Disabled = false;
            _selectedCharacter = type;
            UpdateButtons();
        }

        private void UpdateButtons()
        {
            if (_characterGrid == null)
            {
                return;
            }

            foreach (Node node in _characterGrid.GetChildren())
            {
                if (node is SandboxSelectionItem item && item.CharacterType != _selectedCharacter)
                {
                    item.DeselectItem();
                }
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
                levelGenerator.EnableDebugMode();
                Image image = levelGenerator.Generate(_generatorSettings.Seed);

                GameLevel level = GD.Load<PackedScene>("res://scene/level/GameLevel.tscn").Instantiate<GameLevel>();
                level.Init(image);
                GetTree().Root.AddChild(level);
                GetTree().CurrentScene = level;
                level.SpawnSandboxCharacter(_generatorSettings.SelectedTerrainShape);
                QueueFree();
            }
        }
    }
}