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
        /// <summary>
        /// Emitted when an input button was pressed
        /// </summary>
        [Signal]
        public delegate void InputButtonPressedEventHandler(Button button, string inputKind);

        [Export] private Label _inputActionLabel;
        [Export] private Button _primaryInputButton;
        [Export] private Button _secondaryInputButton;
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
            if (_inputActionLabel == null || _primaryInputButton == null || _secondaryInputButton == null)
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
            if (_isHeader && _inputActionLabel != null)
            {
                _inputActionLabel.Text = _inputText;
            }

            if (_primaryInputButton != null)
            {
                if (_isHeader)
                {
                    _primaryInputButton.Text = _primaryText;
                }
                else
                {
                    _primaryInputButton.Pressed += OnPrimaryButtonPressed;
                }
            }

            if (_secondaryInputButton != null)
            {
                if (_isHeader)
                {
                    _secondaryInputButton.Text = _secondaryText;
                }
                else
                {
                    _secondaryInputButton.Pressed += OnSecondaryButtonPressed;
                }
            }
        }

        /// <summary>
        /// Changes the input information of the item
        /// </summary>
        /// <param name="input">The new input</param>
        /// <param name="inputKind">The kind of input</param>
        public void ChangeInput((string, InputType) input, InputKind inputKind)
        {
            if (inputKind == InputKind.Primary)
            {
                _primaryInput = input.Item1;
                _primaryType = input.Item2;
            }
            else
            {
                _secondaryInput = input.Item1;
                _secondaryType = input.Item2;
            }
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            string actionName = InputDisplayMapper.GetActionName(_inputAction);
            int actionFontSize = actionName.Length > 21 ? 10 : 12;
            _inputActionLabel.AddThemeFontSizeOverride("font_size", actionFontSize);
            _inputActionLabel.Text = actionName;

            _primaryInputButton.Text = GetInputLabelText(_primaryInput, _primaryType);
            _secondaryInputButton.Text = GetInputLabelText(_secondaryInput, _secondaryType);
        }

        private void OnPrimaryButtonPressed()
        {
            if (!_isHeader)
            {
                _ = EmitSignal(SignalName.InputButtonPressed, _primaryInputButton, InputKind.Primary.ToString());
            }
        }

        private void OnSecondaryButtonPressed()
        {
            if (!_isHeader)
            {
                _ = EmitSignal(SignalName.InputButtonPressed, _secondaryInputButton, InputKind.Secondary.ToString());
            }
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