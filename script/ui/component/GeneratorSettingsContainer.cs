using System;

using Godot;

using INTOnlineCoop.Script.Level;

namespace INTOnlineCoop.Script.UI.Component
{
    /// <summary>
    /// Contains settings for level generation
    /// </summary>
    public partial class GeneratorSettingsContainer : VBoxContainer
    {
        [Export] private GridContainer _shapeGrid;
        [Export] private LineEdit _seedEdit;
        [Export] private Theme _buttonTheme;

        private Button _selectedButton;
        private string _seed = "";

        /// <summary>
        /// TerrainShape selected by the user
        /// </summary>
        public TerrainShape SelectedTerrainShape
        {
            get;
            private set;
        }

        /// <summary>
        /// Seed for 
        /// </summary>
        public int Seed
        {
            get
            {
                if (_seed == "")
                {
                    return new Random().Next();
                }

                bool isNumericInput = int.TryParse(_seed, out int seed);
                if (!isNumericInput)
                {
                    seed = unchecked((int)_seed.Hash());
                }

                return seed;
            }
        }

        /// <summary>
        /// Initializes the container
        /// </summary>
        public override void _Ready()
        {
            if (_shapeGrid != null)
            {
                foreach (TerrainShape shape in Enum.GetValues<TerrainShape>())
                {
                    Button button = new()
                    {
                        Theme = _buttonTheme,
                        CustomMinimumSize = new Vector2(80, 45),
                        Name = shape.ToString(),
                        Icon = GD.Load<Texture2D>($"res://assets/texture/level/shape_icon/{shape}.png")
                    };
                    button.Pressed += () => OnTerrainButtonClick(button, shape);
                    _shapeGrid.AddChild(button);
                }
            }

            if (_seedEdit != null)
            {
                _seedEdit.TextChanged += OnSeedLineEditTextChanged;
            }
        }

        private void OnTerrainButtonClick(Button button, TerrainShape shape)
        {
            if (_selectedButton != null)
            {
                _selectedButton.ThemeTypeVariation = "";
            }

            _selectedButton = button;
            _selectedButton.ThemeTypeVariation = "SelectedButton";
            SelectedTerrainShape = shape;
        }

        private void OnSeedLineEditTextChanged(string input)
        {
            _seed = input;
        }
    }
}
