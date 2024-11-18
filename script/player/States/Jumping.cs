using Godot;

namespace INTOnlineCoop.Script.Player.States
{
    /// <summary>
    /// State used when the player is jumping
    /// </summary>
    public partial class Jumping : State
    {
        private const float JumpVelocity = 170f;

        /// <summary>
        /// Apply jump impulse on enter
        /// </summary>
        public override void Enter()
        {
            base.Enter();
            Vector2 velocity = Character.Velocity;
            velocity.Y = -JumpVelocity;
            Character.Velocity = velocity;
        }

        /// <summary>
        /// Updates player movement
        /// </summary>
        /// <param name="delta">Current frame delta</param>
        public override void PhysicProcess(double delta)
        {
            ChangeJumpingAnimation();

            Vector2 velocity = Character.Velocity;

            float inputDirection = Input.GetAxis("walk_left", "walk_right");

            if (Input.IsActionPressed("walk_right"))
            {
                CharacterSprite.FlipH = false;
            }
            else if (Input.IsActionPressed("walk_left"))
            {
                CharacterSprite.FlipH = true;
            }

            velocity.X = inputDirection * Speed;
            velocity.Y += Gravity * (float)delta;

            Character.Velocity = velocity;
            _ = Character.MoveAndSlide();

            GD.Print(CharacterSprite.Animation + " " + CharacterSprite.Frame);
            if (Character.Velocity.Y > 0)
            {
                Character.StateMachine.TransitionTo(AvailableState.Falling);
            }
        }

        /// <summary>
        /// If-statement checks, if current animation is still "JumpingOffGround".
        /// Animation is set during state change from Idle to Jumping or Walking to Jumping
        /// "InAir" Animation will set when condition is triggerd
        /// </summary>
        private void ChangeJumpingAnimation()
        {
            if ((CharacterSprite.Animation == "JumpingOffGround") && (CharacterSprite.Frame == 6))
            {
                //JumpingOffGroundAnimation ist zu Ende -> wechsel auf InAir Animation
                CharacterSprite.Play("InAir");
            }
        }
    }
}