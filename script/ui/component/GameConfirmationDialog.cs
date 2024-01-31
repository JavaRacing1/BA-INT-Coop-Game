using Godot;

namespace OnlineGame
{
    /// <summary>
    /// A pre-configured variant of ConfirmationDialog
    /// </summary>
    public partial class GameConfirmationDialog : ConfirmationDialog
    {
        private static readonly Theme ButtonTheme = GD.Load<Theme>("res://resource/theme/GameConfirmationDialogTheme.tres");

        /// <summary>
        /// Creates a new GameConfimationDialog with the specified texts
        /// </summary>
        /// <param name="title">The window title of the dialog</param>
        /// <param name="description">The description of the dialog</param>
        public GameConfirmationDialog(string title, string description)
        {
            Title = title;
            DialogText = description;

            Size = new(400, 200);
            Name = "GameConfirmationDialog";
            InitialPosition = WindowInitialPosition.CenterMainWindowScreen;

            GetLabel().HorizontalAlignment = HorizontalAlignment.Center;

            SetButtonProperties(GetOkButton(), "OkButton");
            SetButtonProperties(GetCancelButton(), "CancelButton");
        }

        private static void SetButtonProperties(Button button, string themeTypeVariation)
        {
            button.Text = "";
            button.CustomMinimumSize = new(100, 50);
            button.Theme = ButtonTheme;
            button.ThemeTypeVariation = themeTypeVariation;
        }
    }
}