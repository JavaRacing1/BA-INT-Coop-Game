using Godot;

namespace INTOnlineCoop.Script.Player
{
    /// <summary>
    /// Represents a player state
    /// </summary>
    public abstract partial class State : Node
    {
        /// <summary>
        /// Speed of the player
        /// </summary>
        protected const float Speed = 50f; //Spielergeschwindigkeit

        /// <summary>
        /// Gravity applied to the player
        /// </summary>
        protected const float Gravity = 300f; //Gravitation

        /// <summary>
        /// Character which uses the state
        /// </summary>
        [Export]
        protected PlayerCharacter Character { get; private set; } //Spieler initialisieren

        /// <summary>
        /// reference to the usage of the AnimationPlayer build into AnimatedSprite2D with his SpriteFrames and Animation options
        /// </summary>
        [Export]
        protected AnimatedSprite2D CharacterSprite { get; private set; }

        /// <summary>
        /// Enters the state
        /// </summary>
        public virtual void Enter()
        {
            GD.Print($"Entering {Name} state");
        }

        /// <summary>
        /// Runs physic processes
        /// </summary>
        /// <param name="delta">Current Frame-delta</param>
        public virtual void PhysicProcess(double delta)
        {
        }

        /// <summary>
        /// Handles input event 
        /// </summary>
        /// <param name="inputEvent">Input event</param>
        public virtual void HandleInput(InputEvent inputEvent)
        {
        }
    }
}