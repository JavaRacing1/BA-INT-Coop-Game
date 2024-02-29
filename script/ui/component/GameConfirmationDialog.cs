using Godot;

namespace OnlineGame
{
    /// <summary>
    /// A pre-configured variant of ConfirmationDialog
    /// </summary>
    public partial class GameConfirmationDialog : ConfirmationDialog
    {
        private static readonly Theme DialogTheme = GD.Load<Theme>("res://resource/theme/MainTheme.tres");

        /// <summary>
        /// Creates a new GameConfimationDialog with the specified texts
        /// </summary>
        /// <param name="title">The window title of the dialog</param>
        /// <param name="description">The description of the dialog</param>
        public GameConfirmationDialog(string title, string description)
        {
            Title = title;
            DialogText = description;

            Theme = DialogTheme;
            Size = new(200, 100);
            Name = "GameConfirmationDialog";
            InitialPosition = WindowInitialPosition.CenterMainWindowScreen;
            GetLabel().AddThemeFontSizeOverride("font_size", 10);
            GetLabel().AutowrapMode = TextServer.AutowrapMode.WordSmart;
            GetLabel().HorizontalAlignment = HorizontalAlignment.Center;

            SetButtonProperties(GetOkButton(), "OkButton");
            SetButtonProperties(GetCancelButton(), "CancelButton");
        }

        private static void SetButtonProperties(Button button, string themeTypeVariation)
        {
            button.Text = "";
            button.CustomMinimumSize = new(40, 20);
            button.Theme = DialogTheme;
            button.ThemeTypeVariation = themeTypeVariation;
        }
    }
}