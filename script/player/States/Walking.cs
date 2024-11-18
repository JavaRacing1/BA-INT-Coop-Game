using Godot;

using INTOnlineCoop.Script.Util;

namespace INTOnlineCoop.Script.Player.States
{
    /// <summary>
    /// State used when the character is walking
    /// </summary>
    public partial class Walking : State
    {
        /// <summary>
        /// True if the client just jumped
        /// </summary>
        [Export]
        private bool Jumped { get; set; }

        /// <summary>
        /// Movement of the player
        /// </summary>
        [Export]
        private float Direction { get; set; }

        /// <summary>
        /// Handles walking input
        /// </summary>
        /// <param name="delta">Current frame delta</param>
        public override void HandleInput(double delta)
        {
            Direction = InputBlocker.GetAxis(Character, "walk_left", "walk_right");

            if (InputBlocker.IsActionJustPressed(Character, "jump"))
            {
                Jumped = true;
            }
        }

        /// <summary>
        /// Changes animations and states
        /// </summary>
        /// <param name="delta"></param>
        public override void ChangeAnimationsAndStates(double delta)
        {
            if (!Mathf.IsEqualApprox(Direction, 0))
            {
                CharacterSprite.FlipH = Direction < 0;
            }

            if (!Character.IsOnFloor())
            {
                CharacterSprite.Stop();
                CharacterSprite.Animation = "InAir";
                CharacterSprite.Frame = 1;
                CharacterSprite.Play("InAir");
                Character.StateMachine.TransitionTo(AvailableState.Falling);
            }
            else if (Jumped)
            {
                CharacterSprite.Stop();
                CharacterSprite.Play("JumpingOffGround");
                Jumped = false;
                Character.StateMachine.TransitionTo(AvailableState.Jumping);
            }
            else if (Mathf.IsEqualApprox(Direction, 0))
            {
                CharacterSprite.Stop();
                Character.StateMachine.TransitionTo(AvailableState.Idle);
            }
        }

        /// <summary>
        /// Updates player movement
        /// </summary>
        /// <param name="delta">Current frame delta</param>
        public override void PhysicProcess(double delta)
        {
            Vector2 velocity = Character.Velocity;
            velocity.X = Direction * Speed;
            Direction = 0.0f;
            Character.Velocity = velocity;
            _ = Character.MoveAndSlide();
        }
    }
}