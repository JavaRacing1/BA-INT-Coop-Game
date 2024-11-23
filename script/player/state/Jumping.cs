using Godot;

using INTOnlineCoop.Script.Level;

namespace INTOnlineCoop.Script.Player.States
{
    /// <summary>
    /// State used when the player is jumping
    /// </summary>
    public partial class Jumping : State
    {
        private const float JumpVelocity = 180f;

        /// <summary>
        /// Handles jumping input
        /// </summary>
        /// <param name="delta">Current frame delta</param>
        public override void HandleInput(double delta)
        {
            if (!Multiplayer.HasMultiplayerPeer() || Character.PeerId != Multiplayer.GetUniqueId())
            {
                return;
            }

            if (GameLevel.IsInputBlocked || Character.IsBlocked)
            {
                StateMachine.Direction = 0;
                return;
            }

            StateMachine.Direction = Input.GetAxis("walk_left", "walk_right");
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

            if (!Mathf.IsEqualApprox(StateMachine.Direction, 0))
            {
                CharacterSprite.FlipH = StateMachine.Direction < 0;
                Character.UpdateWeaponDirection();
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
            if (StateMachine.Jumped)
            {
                velocity.Y = -JumpVelocity;
                StateMachine.Jumped = false;
            }

            velocity.X = StateMachine.Direction * Speed;
            velocity.Y += Gravity * (float)delta;
            Character.Velocity = velocity;

            _ = Character.MoveAndSlide();
        }
    }
}