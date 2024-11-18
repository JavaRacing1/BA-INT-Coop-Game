using Godot;

using INTOnlineCoop.Script.Player;

namespace INTOnlineCoop.Script.UI.Component
{
    /// <summary>
    /// Selection item used for configuring the sandbox mode
    /// </summary>
    public partial class SandboxSelectionItem : Button
    {
        private static readonly StyleBoxFlat SelectedBox = new() { BgColor = new Color(0.7f, 0.7f, 0.7f), };

        [Export] private TextureRect _texture;

        /// <summary>
        /// Current character type of the item
        /// </summary>
        public CharacterType CharacterType { get; private set; }

        private bool _isSelected;

        /// <summary>
        /// Emitted when the character is selected/unselected
        /// </summary>
        [Signal]
        public delegate void SelectedCharacterChangedEventHandler(CharacterType type);

        /// <summary>
        /// Changes the character of the item
        /// </summary>
        /// <param name="type">New character type</param>
        public void SetCharacter(CharacterType type)
        {
            if (_texture == null || type == CharacterType.None)
            {
                return;
            }

            CharacterType = type;
            _texture.Texture = type.BodyTexture;
        }

        /// <summary>
        /// Removes the selected status of the item
        /// </summary>
        public void DeselectItem()
        {
            if (!_isSelected)
            {
                return;
            }

            _isSelected = false;
            RemoveThemeStyleboxOverride("normal");
            RemoveThemeStyleboxOverride("pressed");
            RemoveThemeStyleboxOverride("hover");
        }

        private void OnButtonPressed()
        {
            if (_isSelected)
            {
                return;
            }

            _isSelected = true;
            AddThemeStyleboxOverride("normal", SelectedBox);
            AddThemeStyleboxOverride("pressed", SelectedBox);
            AddThemeStyleboxOverride("hover", SelectedBox);
            Error error = EmitSignal(SignalName.SelectedCharacterChanged, CharacterType);
            if (error != Error.Ok)
            {
                GD.PrintErr("Could not emit SelectedCharacterChanged event: " + error);
            }
        }
    }
}