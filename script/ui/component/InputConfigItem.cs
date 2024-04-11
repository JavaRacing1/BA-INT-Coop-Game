using System;

using Godot;

using INTOnlineCoop.Script.Singleton;
using INTOnlineCoop.Script.Util;

namespace INTOnlineCoop.Script.UI.Component
{
    /// <summary>
    /// Item to configure control keys for an InputAction
    /// </summary>
    public partial class InputConfigItem : PanelContainer
    {
        [Export] private Label _inputActionLabel;
        [Export] private Label _primaryInputLabel;
        [Export] private Label _secondaryInputLabel;
        [Export] private bool _isHeader;
        [Export] private string _inputText = "";
        [Export] private string _primaryText = "";
        [Export] private string _secondaryText = "";
        private string _inputAction;
        private string _primaryInput;
        private InputType _primaryType;
        private string _secondaryInput;
        private InputType _secondaryType;

        /// <summary>
        /// Initializes the config item
        /// </summary>
        /// <param name="action">The name of the InputAction</param>
        /// <param name="primaryInput">The primary input for the action</param>
        /// <param name="secondaryInput">The secondary input for the action</param>
        public void Init(string action, (string, InputType) primaryInput, (string, InputType) secondaryInput)
        {
            _inputAction = action;
            _primaryInput = primaryInput.Item1;
            _primaryType = primaryInput.Item2;
            _secondaryInput = secondaryInput.Item1;
            _secondaryType = secondaryInput.Item2;
            if (_inputActionLabel == null || _primaryInputLabel == null || _secondaryInputLabel == null)
            {
                return;
            }
            UpdateDisplay();
        }

        /// <summary>
        /// Initializes the texts for 
        /// </summary>
        public override void _Ready()
        {
            if (!_isHeader)
            {
                return;
            }

            if (_inputActionLabel != null)
            {
                _inputActionLabel.Text = _inputText;
            }

            if (_primaryInputLabel != null)
            {
                _primaryInputLabel.Text = _primaryText;
            }

            if (_secondaryInputLabel != null)
            {
                _secondaryInputLabel.Text = _secondaryText;
            }
        }

        private void UpdateDisplay()
        {
            string actionName = InputDisplayMapper.GetActionName(_inputAction);
            int actionFontSize = actionName.Length > 21 ? 10 : 12;
            _inputActionLabel.AddThemeFontSizeOverride("font_size", actionFontSize);
            _inputActionLabel.Text = actionName;

            _primaryInputLabel.Text = GetInputLabelText(_primaryInput, _primaryType);
            _secondaryInputLabel.Text = GetInputLabelText(_secondaryInput, _secondaryType);
        }

        private static string GetInputLabelText(string input, InputType type)
        {
            if (type == InputType.Key)
            {
                Key key = Enum.Parse<Key>(input);
                return InputDisplayMapper.GetKeyName(key);
            }
            if (type == InputType.JoyButton)
            {
                JoyButton button = Enum.Parse<JoyButton>(input);
                return InputDisplayMapper.GetJoyButtonName(button);
            }

            float value = input[0].Equals('+') ? 1.0f : -1.0f;
            JoyAxis axis = Enum.Parse<JoyAxis>(input[1..]);
            return InputDisplayMapper.GetJoyAxisName(axis, value);
        }
    }
}