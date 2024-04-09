using System;

using Godot;

using INTOnlineCoop.Script.Singleton;
using INTOnlineCoop.Script.UI.Component;

namespace INTOnlineCoop.Script.UI.Screen
{
    /// <summary>
    /// Screen for the user to change his settings
    /// </summary>
    public partial class SettingsWindow : CanvasLayer
    {
        [Export] private OptionButton _displayModeButton;
        [Export] private CheckBox _particleCheckBox;
        [Export] private HSlider _masterVolumeSlider;
        [Export] private HSlider _musicVolumeSlider;
        [Export] private HSlider _effectVolumeSlider;
        [Export] private Label _masterCurrentVolumeLabel;
        [Export] private Label _musicCurrentVolumeLabel;
        [Export] private Label _effectCurrentVolumeLabel;
        [Export] private CheckBox _controlHintCheckBox;
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
                _displayModeButton.ItemSelected += index => _playerSettingsData.SetDisplayMode((DisplayMode)index);
            }

            if (_particleCheckBox != null)
            {
                _particleCheckBox.Toggled += toggled => _playerSettingsData.SetParticlesEnabled(toggled);
            }

            if (_masterVolumeSlider != null && _masterCurrentVolumeLabel != null)
            {
                _masterVolumeSlider.ValueChanged += volume =>
                {
                    _playerSettingsData.SetMasterVolume((int)volume);
                    _masterCurrentVolumeLabel.Text = Convert.ToString((int)volume);
                };
            }

            if (_musicVolumeSlider != null && _musicCurrentVolumeLabel != null)
            {
                _musicVolumeSlider.ValueChanged += volume =>
                {
                    _playerSettingsData.SetMusicVolume((int)volume);
                    _musicCurrentVolumeLabel.Text = Convert.ToString((int)volume);
                };
            }

            if (_effectVolumeSlider != null && _effectCurrentVolumeLabel != null)
            {
                _effectVolumeSlider.ValueChanged += volume =>
                {
                    _playerSettingsData.SetEffectVolume((int)volume);
                    _effectCurrentVolumeLabel.Text = Convert.ToString((int)volume);
                };
            }

            if (_controlHintCheckBox != null)
            {
                _controlHintCheckBox.Toggled += toggled => _playerSettingsData.SetControlHintVisibility(toggled);
            }

            UpdateSettings();
        }

        private void UpdateSettings()
        {
            if (_displayModeButton != null)
            {
                _displayModeButton.Selected = (int)_playerSettingsData.SelectedDisplayMode;
            }

            if (_particleCheckBox != null)
            {
                _particleCheckBox.ButtonPressed = _playerSettingsData.AreParticlesEnabled;
            }

            if (_masterVolumeSlider != null && _masterCurrentVolumeLabel != null)
            {
                _masterVolumeSlider.Value = _playerSettingsData.MasterVolume;
                _masterCurrentVolumeLabel.Text = Convert.ToString(_playerSettingsData.MasterVolume);
            }

            if (_musicVolumeSlider != null && _musicCurrentVolumeLabel != null)
            {
                _musicVolumeSlider.Value = _playerSettingsData.MusicVolume;
                _musicCurrentVolumeLabel.Text = Convert.ToString(_playerSettingsData.MusicVolume);
            }

            if (_effectVolumeSlider != null && _effectCurrentVolumeLabel != null)
            {
                _effectVolumeSlider.Value = _playerSettingsData.EffectVolume;
                _effectCurrentVolumeLabel.Text = Convert.ToString(_playerSettingsData.EffectVolume);
            }

            if (_controlHintCheckBox != null)
            {
                _controlHintCheckBox.ButtonPressed = _playerSettingsData.ShowControlHints;
            }
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
                _cancelDialog = new("Einstellungen verlassen",
                    "Möchtest du die Einstellungen wirklich verlassen? Deine Änderungen werden nicht gespeichert");
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
                _discardDialog = new("Änderungen verwerfen",
                    "Möchtest du deine ungespeicherten Änderungen wirklich verwerfen?");
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