using Godot;

namespace OnlineGame
{
    /// <summary>
    /// Screen for the user to change his settings
    /// </summary>
    public partial class SettingsWindow : CanvasLayer
    {

        [Export]
        private OptionButton _displayModeButton;
        private PlayerSettingsData _playerSettingsData;

        /// <summary>
        /// Initializes the settings window
        /// </summary>
        public override void _Ready()
        {
            _playerSettingsData = GetNode<PlayerSettingsData>("/root/PlayerSettingsData");
            if (_displayModeButton != null)
            {
                _displayModeButton.ItemSelected += OnDisplayModeItemSelected;
            }
            UpdateSettings();
        }

        private void UpdateSettings()
        {
            if (_displayModeButton != null)
            {
                _displayModeButton.Selected = (int)_playerSettingsData.SelectedDisplayMode;
            }
        }

        private void OnDisplayModeItemSelected(long index)
        {
            _playerSettingsData.SetDisplayMode((DisplayMode)index);
        }

        private void OnCancelButtonPressed()
        {
            //TODO: Abfrage
            _playerSettingsData.DiscardChanges();
            UpdateSettings();
            Visible = false;
        }

        private void OnDiscardButtonPressed()
        {
            //TODO: Abfrage
            _playerSettingsData.DiscardChanges();
            UpdateSettings();
        }

        private void OnApplyButtonPressed()
        {
            _playerSettingsData.ApplyChanges();
            Visible = false;
        }
    }
}

