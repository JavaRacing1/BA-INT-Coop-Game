using Godot;

using INTOnlineCoop.Script.Player;
using INTOnlineCoop.Script.Singleton;

namespace INTOnlineCoop.Script.Level
{
    /// <summary>
    /// 
    /// </summary>
    public partial class GameLevelUserInterface : CanvasLayer
    {
        // Timer Nodes exportieren
        [Export] private Timer _timer;
        [Export] private Label _labelTime;

        // PlayerCharacter Listennodes
        [Export] private RichTextLabel _labelPlayer1;
        [Export] private RichTextLabel _labelPlayer2;
        [Export] private Sprite2D[] _spritesPlayer1;
        [Export] private Sprite2D[] _spritesPlayer2;

        [Export] private Label _notificationLabel;

        // Waffenbutton Node Liste
        [Export] private HBoxContainer _weaponContainer;
        [Export] private TextureButton _textureButtonBazooka;
        [Export] private TextureButton _textureButtonPistol;
        [Export] private TextureButton _textureButtonShotgun;
        [Export] private TextureButton _textureButtonSniper;

        //Hilfsvariable für Todeszustände der SPielfiguren
        //private readonly bool[] _player1CharacterDead;
        //private readonly bool[] _player2CharacterDead;

        //Hilfsvariable Anzahl an möglichen Schüssen von Waffen
        //(x = -1: undendlich, x = 0: alle Schüsse aufgebraucht, x>0: Waffe abschießbereit)
        private int _bazzokaUsageNumber = -1;
        private int _pistolUsageNumber;
        private int _shotgunUsageNumber;
        private int _sniperUsageNumber;

        // Hilfvariable um zu erkennen, ob geschossen wurde oder nicht, nachdem Waffe ausgewählt wurde
        private bool _shotTaken;

        /// <summary>
        /// Zuweisen der privaten Variablen auf alle genutzen UI Nodes
        /// und Farbe initialisieren bei Timer Label
        /// </summary>
        public override void _Ready()
        {
            // Setze Default Farbe für Timer Label
            _labelTime.AddThemeColorOverride("font_color", Color.Color8(255, 255, 255));

            //Spielfigurenzustände zu Beginn auf "lebend" setzen
            // _isFigureA1Dead = false 

            foreach (PlayerData data in MultiplayerLobby.Instance.GetPlayerData())
            {
                int playerNumber = data.PlayerNumber;
                UpdateCharacterIcons(playerNumber, data.Characters);
                SetPlayerName(playerNumber, data.Name);
            }

            //aktivieren aller Waffen Button
            //Bazzoka = unendliche Schüsse, Sniper und Shotgun auf x zum Start festlegen,
            //Pistole wegen möglichen Singel oder Tripel-Schuss unterscheiden von Anzhal Schüssen
        }

        /// <summary>
        /// Update timer label
        /// </summary>
        /// <param name="delta">Current frame delta</param>
        public override void _Process(double delta)
        {
            // Zeit-Label aktualisieren
            UpdateTimeLabel();
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateTimeLabel()
        {
            // Verbleibende Zeit vom Timer abrufen
            float timeLeft = (float)_timer.TimeLeft;
            if (timeLeft != 0 && !_labelTime.Visible)
            {
                _labelTime.Visible = true;
            }

            //Verändere Color vom TimeLable, wenn timeLeft unter 10 s
            _labelTime.AddThemeColorOverride("font_color",
                timeLeft <= 10f ? Color.Color8(255, 0, 0) : Color.Color8(255, 255, 255));

            // Zeit formatieren (Minuten:Sekunden) und auf Label anzeigen
            _labelTime.Text = FormatTime(timeLeft);
        }

        [Rpc(CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
        private void HideTimerLabel()
        {
            _labelTime.Visible = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        private static string FormatTime(float time)
        {
            int minutes = (int)(time / 60);
            int seconds = (int)(time % 60);
            return $"{minutes:00}:{seconds:00}";
        }

        /// <summary>
        /// Displays the turn notification
        /// </summary>
        /// <param name="playerName">Name of the player</param>
        /// <param name="playerNumber">Player number</param>
        public void DisplayTurnNotification(string playerName, int playerNumber)
        {
            if (_notificationLabel == null)
            {
                return;
            }

            _notificationLabel.Visible = true;
            _notificationLabel.Text = playerName + " ist am Zug!";
            _notificationLabel.AddThemeColorOverride("font_color",
                playerNumber == 1 ? Color.Color8(255, 70, 54) : Color.Color8(97, 146, 255));
            GetTree().CreateTimer(5).Timeout += () => _notificationLabel.Visible = false;
        }

        /// <summary>
        /// Updates the display status of the weapons
        /// </summary>
        /// <param name="playerNumber">Number of the player who is on turn</param>
        public void DisplayWeapons(int playerNumber)
        {
            long peerId = Multiplayer.GetUniqueId();
            if (peerId == 1 || _weaponContainer == null)
            {
                return;
            }

            _weaponContainer.Visible = MultiplayerLobby.Instance.GetPlayerData(peerId).PlayerNumber == playerNumber;
        }

        /// <summary>
        /// Changes the character icons of a player
        /// </summary>
        /// <param name="playerNumber">Number of the player</param>
        /// <param name="characters">Current characters</param>
        public void UpdateCharacterIcons(int playerNumber, CharacterType[] characters)
        {
            Sprite2D[] playerSprites = playerNumber == 1 ? _spritesPlayer1 : _spritesPlayer2;
            if (characters.Length != 4 || playerSprites.Length != 4)
            {
                GD.PrintErr("Wrong number of characters in GameLevel UI");
                return;
            }

            for (int i = 0; i < playerSprites.Length; i++)
            {
                CharacterType type = characters[i];
                playerSprites[i].Texture = type == CharacterType.None
                    ? GD.Load<Texture2D>("res://assets/texture/PlayerDead.png")
                    : characters[i].HeadTexture;
            }
        }

        /// <summary>
        /// Sets the name of a player
        /// </summary>
        /// <param name="playerNumber">Number of the player</param>
        /// <param name="playerName">Username</param>
        public void SetPlayerName(int playerNumber, string playerName)
        {
            RichTextLabel playerLabel = playerNumber == 1 ? _labelPlayer1 : _labelPlayer2;
            if (playerLabel == null)
            {
                return;
            }

            long currentPeer = Multiplayer.GetUniqueId();
            if (currentPeer == 1)
            {
                return;
            }

            bool isCurrentClient = MultiplayerLobby.Instance.GetPlayerData(currentPeer).PlayerNumber == playerNumber;
            string prefix = isCurrentClient ? "[right][u]" : "[right]";
            playerLabel.Text = prefix + playerName;
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnBazzokaButtonPressed()
        {
            //bool shotTaken = false;
            GD.Print("Bazzoka ausgewählt.");
            if (_shotTaken)
            {
                GD.Print("Bazzoka abgefeuert.");
                _shotTaken = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnPistolButtonPressed()
        {
            GD.Print("Pistole ausgewählt.");
            if (_shotTaken)
            {
                GD.Print("Pistole abgefeuert.");
                _pistolUsageNumber--;
                _shotTaken = false;
                if (_pistolUsageNumber == 0)
                {
                    _textureButtonPistol.Disabled = true;
                    GD.Print("keine Pistolenschüsse mehr vorhanden.");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnShotgunButtonPressed()
        {
            GD.Print("Shotgun ausgewählt.");
            if (_shotTaken)
            {
                GD.Print("Shotgun abgefeuert.");
                _shotgunUsageNumber--;
                _shotTaken = false;
                if (_shotgunUsageNumber == 0)
                {
                    _textureButtonShotgun.Disabled = true;
                    GD.Print("keine Shotgunschüsse mehr vorhanden.");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnSniperButtonPressed()
        {
            GD.Print("Sniper ausgewählt.");
            if (_shotTaken)
            {
                GD.Print("Sniper abgefeuert.");
                _sniperUsageNumber--;
                _shotTaken = false;
                if (_sniperUsageNumber == 0)
                {
                    _textureButtonSniper.Disabled = true;
                    GD.Print("keine Sniperschüsse mehr vorhanden.");
                }
            }
        }
    }
}