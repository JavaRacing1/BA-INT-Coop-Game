using Godot;

namespace INTOnlineCoop.Script.Player
{
    /// <summary>
    /// Character controlled by the player
    /// </summary>
    public partial class PlayerCharacter : CharacterBody2D
    {
        [Export] private AnimatedSprite2D _sprite;

        /// <summary>
        /// Current StateMachine instance
        /// </summary>
        [Export]
        public StateMachine StateMachine { get; private set; }

        /// <summary>
        /// Current type used by the character
        /// </summary>
        public CharacterType Type { get; private set; }

        /// <summary>
        /// Peer ID of the controlling player
        /// </summary>
        [Export]
        public int PeerId { get; private set; }

        /// <summary>
        /// True if the player is blocked from inputs
        /// </summary>
        public bool IsBlocked { get; set; }

        /// <summary>
        /// Initializes the character
        /// </summary>
        /// <param name="position">Character position</param>
        /// <param name="type">Type of the character</param>
        public void Init(Vector2 position, CharacterType type)
        {
            Position = position;
            Type = type;

            if (_sprite == null || type.SpriteFrames == null)
            {
                return;
            }

            _sprite.SpriteFrames = type.SpriteFrames;
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
    }
}