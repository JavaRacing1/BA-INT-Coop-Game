using Godot;

namespace OnlineGame
{
    /// <summary>
    /// The start scene of the game
    /// </summary>
    public partial class MainMenu : Control
    {
        private GameConfirmationDialog _exitDialog;

        private void OnExitButtonPressed()
        {
            if (_exitDialog == null)
            {
                _exitDialog = new("Spiel verlassen", "Möchtest du wirklich das Spiel verlassen?");
                _exitDialog.GetOkButton().Pressed += () => GetTree().Quit();
                AddChild(_exitDialog);
            }
            _exitDialog.Visible = true;
        }

        private void OnCreditsButtonPressed()
        {
            //TODO: Add credits
        }

        private void OnSettingsButtonPressed()
        {
            //TODO: Add settings menu
        }
    }
}