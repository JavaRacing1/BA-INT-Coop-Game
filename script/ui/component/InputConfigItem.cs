using Godot;

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
        private Key _primaryKey;
        private Key _secondaryKey;

        /// <summary>
        /// Initializes the config item
        /// </summary>
        /// <param name="action">The name of the InputAction</param>
        /// <param name="primaryKey">The primary key for the action</param>
        /// <param name="secondaryKey">The secondary key for the action</param>
        public void Init(string action, Key primaryKey, Key secondaryKey)
        {
            _inputAction = action;
            _primaryKey = primaryKey;
            _secondaryKey = secondaryKey;
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

            _primaryInputLabel.Text = InputDisplayMapper.GetKeyName(_primaryKey);
            _secondaryInputLabel.Text = InputDisplayMapper.GetKeyName(_secondaryKey);
        }
    }
}