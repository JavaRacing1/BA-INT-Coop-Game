using Godot;

using INTOnlineCoop.Script.Level;
using INTOnlineCoop.Script.Singleton;

namespace INTOnlineCoop.Script.Player
{
    /// <summary>
    /// Character controlled by the player
    /// </summary>
    public partial class PlayerCharacter : CharacterBody2D
    {
        /// <summary>
        /// True if the player is blocked from inputs
        /// </summary>
        [Export] public bool IsBlocked;

        [Export] private AnimatedSprite2D _sprite;
        [Export] private Label _healthLabel;
        [Export] private TextureRect _characterIcon;
        [Export] private string _type;
        [Export] private int _health = 100;

        private long _peerId;

        /// <summary>
        /// Current StateMachine instance
        /// </summary>
        [Export]
        public StateMachine StateMachine { get; private set; }

        /// <summary>
        /// Peer ID of the controlling player
        /// </summary>
        [Export]
        public long PeerId
        {
            get => _peerId;
            private set
            {
                _peerId = value;
                StateMachine?.SetMultiplayerAuthority((int)_peerId);
                ChangeHealthLabelColor(_peerId);
            }
        }

        /// <summary>
        /// Current type used by the character
        /// </summary>
        public CharacterType Type => CharacterType.FromName(_type);

        /// <summary>
        /// True if the character has the correct textures
        /// </summary>
        public bool TexturesLoaded { get; private set; }

        /// <summary>
        /// Emitted when the player died
        /// </summary>
        [Signal]
        public delegate void PlayerDiedEventHandler(PlayerCharacter character);

        /// <summary>
        /// Current health of the player
        /// </summary>
        public int Health
        {
            get => _health;
            set
            {
                _health = value;
                _healthLabel.Text = $"{_health}";
            }
        }

        /// <summary>
        /// Initializes the character
        /// </summary>
        /// <param name="position">Character position</param>
        /// <param name="type">Type of the character</param>
        /// <param name="peerId">Peer ID of the controlling player</param>
        public void Init(Vector2 position, CharacterType type, long peerId)
        {
            Position = position;
            _type = type.Name;
            PeerId = peerId;
        }

        /// <summary>
        /// Loads the textures of the selected character
        /// </summary>
        public void LoadTextures()
        {
            if (_sprite == null || Type.SpriteFrames == null)
            {
                return;
            }

            _characterIcon.Texture = Type.HeadTexture;
            _sprite.SpriteFrames = Type.SpriteFrames;
            TexturesLoaded = true;
        }

        /// <summary>
        /// Initializes the state machine
        /// </summary>
        public override void _Ready()
        {
            if (StateMachine == null)
            {
                return;
            }

            StateMachine.TransitionTo(AvailableState.Idle); // Initialer Zustand
        }

        //SetPlayerTyp Methode()
        //Lese CharacterTyp enum auf ausgewählte Figuren von User
        //Export Funktion zu AnimatedSprite2D Texture - Zuweisung der SpriteSheet Eingenschaft passend zum Namen der Figuren
        //zwischenspeichern in eigener Variable

        /// <summary>
        /// Redirects physic and movement updates to states
        /// </summary>
        /// <param name="delta">Current frame delta</param>
        public override void _PhysicsProcess(double delta)
        {
            StateMachine.CurrentState.PhysicProcess(delta);
        }

        /// <summary>
        /// Redirects process for input and animation handling to states
        /// </summary>
        /// <param name="delta">Current frame delta</param>
        public override void _Process(double delta)
        {
            StateMachine.CurrentState.HandleInput(delta);
            StateMachine.CurrentState.ChangeAnimationsAndStates(delta);
        }

        private void ChangeHealthLabelColor(long controllingPeerId)
        {
            if (_healthLabel == null)
            {
                return;
            }

            PlayerData playerData = MultiplayerLobby.Instance.GetPlayerData(controllingPeerId);
            _healthLabel.AddThemeColorOverride("font_color",
                playerData.PlayerNumber == 1
                    ? GameLevelUserInterface.PlayerOneColor
                    : GameLevelUserInterface.PlayerTwoColor);
        }

        /// <summary>
        /// Displays the health label
        /// </summary>
        public void DisplayHealth()
        {
            if (Health > 0)
            {
                _healthLabel.Visible = true;
            }
        }

        /// <summary>
        /// Displays the character icon
        /// </summary>
        public void DisplayCharacterIcon()
        {
            if (Health > 0)
            {
                _characterIcon.Visible = true;
            }
        }

        /// <summary>
        /// Hides the health label
        /// </summary>
        public void HideHealth()
        {
            _healthLabel.Visible = false;
        }

        /// <summary>
        /// Hides the character icon
        /// </summary>
        public void HideCharacterIcon()
        {
            _characterIcon.Visible = false;
        }

        /// <summary>
        /// Damages the player
        /// </summary>
        /// <param name="damageAmount"></param>
        [Rpc(CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
        public void Damage(int damageAmount)
        {
            if (Health <= 0)
            {
                return;
            }

            Health -= damageAmount;
            if (Health <= 0)
            {
                _ = EmitSignal(SignalName.PlayerDied, this);
            }
        }
    }
}