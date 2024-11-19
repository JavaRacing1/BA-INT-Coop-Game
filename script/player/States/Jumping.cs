using Godot;

using INTOnlineCoop.Script.Level;

namespace INTOnlineCoop.Script.Player.States
{
    /// <summary>
    /// State used when the player is jumping
    /// </summary>
    public partial class Jumping : State
    {
        private const float JumpVelocity = 170f;

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
        /// Apply jump impulse on enter
        /// </summary>
        public override void Enter()
        {
            base.Enter();
            Jumped = true;
        }

        /// <summary>
        /// Handles jumping input
        /// </summary>
        /// <param name="delta">Current frame delta</param>
        public override void HandleInput(double delta)
        {
            if (GameLevel.IsInputBlocked || Character.IsBlocked || Character.PeerId != Multiplayer.GetUniqueId())
            {
                return;
            }

            Direction = Input.GetAxis("walk_left", "walk_right");
        }

        /// <summary>
        /// Changes jumping animations
        /// </summary>
        /// <param name="delta">Current frame delta</param>
        public override void ChangeAnimationsAndStates(double delta)
        {
            if ((CharacterSprite.Animation == "JumpingOffGround") && (CharacterSprite.Frame == 6))
            {
                //JumpingOffGroundAnimation ist zu Ende -> wechsel auf InAir Animation
                CharacterSprite.Play("InAir");
            }

            if (!Mathf.IsEqualApprox(Direction, 0))
            {
                CharacterSprite.FlipH = Direction < 0;
            }

            if (Character.Velocity.Y > 0)
            {
                Character.StateMachine.TransitionTo(AvailableState.Falling);
            }
        }

        /// <summary>
        /// Updates player movement
        /// </summary>
        /// <param name="delta">Current frame delta</param>
        public override void PhysicProcess(double delta)
        {
            Vector2 velocity = Character.Velocity;
            if (Jumped)
            {
                velocity.Y = -JumpVelocity;
            }

            Jumped = false;
            velocity.X = Direction * Speed;
            velocity.Y += Gravity * (float)delta;
            Character.Velocity = velocity;

            _ = Character.MoveAndSlide();
        }
    }
}