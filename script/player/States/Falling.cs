using Godot;

namespace INTOnlineCoop.Script.Player.States
{
    /// <summary>
    /// State used when player is falling
    /// </summary>
    public partial class Falling : State
    {
        /// <summary>
        /// reference to the usage of the AnimationPlayer build into AnimatedSprite2D with his SpriteFrames and Animation options
        /// </summary>
        [Export] private AnimatedSprite2D _figureAnimation;


        /// <summary>
        /// If-statement checks, if current animation is still "InAir".
        /// Animation is set during state change from Idle to Falling or Walking to Falling
        /// "InAir" Animation will loop until condition is triggerd
        /// </summary>
        private void ChangeingFallingAnimation()
        {

            if ((_figureAnimation.Animation == "InAir") && Character.IsOnFloor())
            {
                //InAir Animation muss beendet werden, da Kollision mit Boden erkannt
                //-> wechsel auf LandingOnGround Animation
                _figureAnimation.Stop();
                _figureAnimation.Play("LandingOnGround");
            }
        }
        /// <summary>
        /// Updates player movement
        /// </summary>
        /// <param name="delta">Current frame delta</param>
        public override void PhysicProcess(double delta)
        {
            ChangeingFallingAnimation();

            Vector2 velocity = Character.Velocity;

            float inputDirection = Input.GetAxis("walk_left", "walk_right");

            if (Input.IsActionPressed("walk_right"))
            {
                _figureAnimation.FlipH = false;
            }
            else if (Input.IsActionPressed("walk_left"))
            {
                _figureAnimation.FlipH = true;
            }

            velocity.X = inputDirection * Speed;
            velocity.Y += Gravity * (float)delta;

            Character.Velocity = velocity;
            _ = Character.MoveAndSlide();

            if (Character.IsOnFloor() && (_figureAnimation.Animation == "LandingOnGround") && (_figureAnimation.Frame == 2))
            {
                Character.StateMachine.TransitionTo(Mathf.IsEqualApprox(inputDirection, 0.0)
                    ? AvailableState.Idle
                    : AvailableState.Walking);
            }
        }
    }
}