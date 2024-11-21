using Godot;

using INTOnlineCoop.Script.Player;

namespace INTOnlineCoop.Script.Level
{
    /// <summary>
    /// 
    /// </summary>
    public partial class UIElements : CanvasLayer
    {
        // Timer Nodes exportieren
        [Export]
        private Timer _timer;
        [Export]
        private Label _labelLeftTime;
        [Export]
        private Label _labelTime;

        // PlayerCharacter Listennodes
        [Export] private Sprite2D[] _spritesPlayer1;
        [Export] private Sprite2D[] _spritesPlayer2;

        // Waffenbutton Node Liste
        [Export]
        private TextureButton _textureButtonBazzoka;
        [Export]
        private TextureButton _textureButtonPistol;
        [Export]
        private TextureButton _textureButtonShotgun;
        [Export]
        private TextureButton _textureButtonSniper;

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
            _labelTime.AddThemeColorOverride("font_color", Color.Color8(255, 255, 255, 255));
            _labelLeftTime.AddThemeColorOverride("font_color", Color.Color8(255, 255, 255, 255));

            //Initialiseriung der für die Runde aktiven Figuren für Spieler 1 und 2 (Interface oben rechts)
            //"PlayerDead" Textur mit jeweilger Head Texture der Spielerfigur erstezen

            //Spielfigurenzustände zu Beginn auf "lebend" setzen
            // _isFigureA1Dead = false 


            //aktivieren aller Waffen Button
            //Bazzoka = unendliche Schüsse, Sniper und Shotgun auf x zum Start festlegen,
            //Pistole wegen möglichen Singel oder Tripel-Schuss unterscheiden von Anzhal Schüssen

            // Timer starten
            StartRound();

        }

        /// <summary>
        /// 
        /// </summary>
        public void StartRound()
        {
            // Timer für 60 Sekunden setzen und starten
            _timer.WaitTime = 60.0f;
            _timer.OneShot = true;
            _timer.Start();


            // Optional: Initiales Update des Labels
            UpdateTimeLabel();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="delta"></param>
        public override void _Process(double delta)
        {


            if (_timer.IsStopped())
            {
                // Falls Timer abgelaufen ist, Rundenende loggen
                GD.Print("Die Runde ist vorbei.");
                return;
            }

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
            //Verändere Color von Lable TiemLeft und TimeLable, wenn timeLeft <= 30 s
            if (timeLeft <= 30f)
            {
                _labelTime.AddThemeColorOverride("font_color", Color.Color8(255, 0, 0, 255));
                _labelLeftTime.AddThemeColorOverride("font_color", Color.Color8(255, 0, 0, 255));
            }
            // Zeit formatieren (Minuten:Sekunden) und auf Label anzeigen
            _labelLeftTime.Text = FormatTime(timeLeft);
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
        /// Set the character textures to Player UI List top right of the camera
        /// </summary>
        /// <param name="characterTypes"></param>
        public void SetCharactersPlayer1(CharacterType[] characterTypes)
        {
            if (_spritesPlayer1 is not { Length: 4 } || characterTypes.Length != 4)
            {
                GD.PrintErr("Sprites array for Player 1 are not initialized.");
                return;
            }

            for (int i = 0; i < 4; i++)
            {
                Sprite2D spritePL1 = _spritesPlayer1[i];
                CharacterType character = characterTypes[i];
                if (character != CharacterType.None)
                {
                    spritePL1.Texture = character.HeadTexture;
                }
            }
        }
        /// <summary>
        /// Set the character textures to Player UI List top right of the camera
        /// </summary>
        /// <param name="characterTypes"></param>
        public void SetCharactersPlayer2(CharacterType[] characterTypes)
        {
            if (_spritesPlayer2 is not { Length: 4 } || characterTypes.Length != 4)
            {
                GD.PrintErr("Sprites array for Player 2 are not initialized.");
                return;
            }

            for (int i = 0; i < 4; i++)
            {
                Sprite2D spritePL2 = _spritesPlayer2[i];
                CharacterType character = characterTypes[i];
                if (character != CharacterType.None)
                {
                    spritePL2.Texture = character.HeadTexture;
                }
            }
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






