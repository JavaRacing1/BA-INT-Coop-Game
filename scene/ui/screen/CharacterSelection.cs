using Godot;

/// <summary>
/// Scene to select player characters (4 for each player, no repeat of characters)
/// </summary>
public partial class CharacterSelection : Node
{
    //lable-Nodes von Charakters
    [Export] private TextureButton _headAmaraButton;
    [Export] private TextureButton _headAthenaButton;
    [Export] private TextureButton _headAxtonButton;
    [Export] private TextureButton _headGaigeButton;
    [Export] private TextureButton _headMajaButton;
    [Export] private TextureButton _headMozeButton;
    [Export] private TextureButton _headNishaButton;
    [Export] private TextureButton _headKriegButton;
    [Export] private TextureButton _headSalvadorButton;
    [Export] private TextureButton _headWhilhelmButton;
    [Export] private TextureButton _headZeroButton;

    //TextureButton-Nodes von Charakterss

    [Export] private Label _amaraLabel;
    [Export] private Label _athenaLabel;
    [Export] private Label _axtonLabel;
    [Export] private Label _gaigeLabel;
    [Export] private Label _majaLabel;
    [Export] private Label _mozeLabel;
    [Export] private Label _nishaLabel;
    [Export] private Label _kriegLabel;
    [Export] private Label _salvadorLabel;
    [Export] private Label _whilhelmLabel;
    [Export] private Label _zeroLabel;

    //PopUp verstecken
    [Export] private Popup _confirmPopUp;

    // Maximal vier Figuren auswählbar
    private const int MaxSelections = 4;

    // Verweise auf Bestätigen und Abbrechen-Buttons
    [Export] private Button _confirmSelectionButton;
    [Export] private Button _returnSelectionButton;
    [Export] private Button _confirmPopupButton;
    [Export] private Button _returnPopupButton;

    /// <summary>
    /// Initialize all label nodes with the text color "white",
    /// deactivate the "confirmSelectionButton" button and make it invisible
    /// </summary>
    public override void _Ready()
    {

        // Setze den Anfangszustand für die Buttons
        _confirmSelectionButton.Visible = false;
        _confirmSelectionButton.Disabled = true;
        _returnSelectionButton.Disabled = false;
        _confirmPopupButton.Disabled = false;
        _returnPopupButton.Disabled = false;
        _headZeroButton.Disabled = false;
        _headWhilhelmButton.Disabled = false;
        _headSalvadorButton.Disabled = false;
        _headNishaButton.Disabled = false;
        _headMozeButton.Disabled = false;
        _headMajaButton.Disabled = false;
        _headKriegButton.Disabled = false;
        _headGaigeButton.Disabled = false;
        _headAxtonButton.Disabled = false;
        _headAthenaButton.Disabled = false;
        _headAmaraButton.Disabled = false;

        //Weieße Lable-Farbe bei Szenenaufruf setzten
        _zeroLabel.Modulate = new Color(1, 1, 1);
        _whilhelmLabel.Modulate = new Color(1, 1, 1);
        _salvadorLabel.Modulate = new Color(1, 1, 1);
        _nishaLabel.Modulate = new Color(1, 1, 1);
        _mozeLabel.Modulate = new Color(1, 1, 1);
        _majaLabel.Modulate = new Color(1, 1, 1);
        _kriegLabel.Modulate = new Color(1, 1, 1);
        _gaigeLabel.Modulate = new Color(1, 1, 1);
        _axtonLabel.Modulate = new Color(1, 1, 1);
        _athenaLabel.Modulate = new Color(1, 1, 1);
        _amaraLabel.Modulate = new Color(1, 1, 1);

        //PopUp verstecken
        _confirmPopUp.Visible = false;
    }

    private void OnAthenaHeadPressed()
    {
        //
    }
};

