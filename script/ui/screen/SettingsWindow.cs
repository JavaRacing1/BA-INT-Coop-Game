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
        private GameConfirmationDialog _cancelDialog;
        private GameConfirmationDialog _discardDialog;

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
            void PressAction()
            {
                _playerSettingsData.DiscardChanges();
                UpdateSettings();
                Visible = false;
            }

            if (_cancelDialog == null)
            {
                _cancelDialog = new("Einstellungen verlassen", "Möchtest du die Einstellungen wirklich verlassen? Deine Änderungen werden nicht gespeichert");
                _cancelDialog.GetOkButton().Pressed += () => PressAction();
                AddChild(_cancelDialog);
            }

            if (_playerSettingsData.HasUnsavedChanges)
            {
                _cancelDialog.Visible = true;
            }
            else
            {
                PressAction();
            }
        }

        private void OnDiscardButtonPressed()
        {
            void PressAction()
            {
                _playerSettingsData.DiscardChanges();
                UpdateSettings();
            }

            if (_discardDialog == null)
            {
                _discardDialog = new("Änderungen verwerfen", "Möchtest du deine ungespeicherten Änderungen wirklich verwerfen?");
                _discardDialog.GetOkButton().Pressed += () => PressAction();
                AddChild(_discardDialog);
            }

            if (_playerSettingsData.HasUnsavedChanges)
            {
                _discardDialog.Visible = true;
            }
            else
            {
                PressAction();
            }
        }

        private void OnApplyButtonPressed()
        {
            _playerSettingsData.ApplyChanges();
            Visible = false;
        }
    }
}

