using Godot;

namespace INTOnlineCoop.Script.Player.States
{
    /// <summary>
    /// State used when the character is walking
    /// </summary>
    public partial class Walking : State
    {
        /// <summary>
        /// reference to the usage of the AnimationPlayer build into AnimatedSprite2D with his SpriteFrames and Animation options
        /// </summary>
        [Export] private AnimatedSprite2D _figureAnimation;

        // private bool _gettingHit;

        /// <summary>
        /// Updates player movement
        /// </summary>
        /// <param name="delta">Current frame delta</param>
        public override void PhysicProcess(double delta)
        {
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

            Character.Velocity = velocity;
            _ = Character.MoveAndSlide();

            if (!Character.IsOnFloor())
            {
                _figureAnimation.Stop();
                _figureAnimation.Animation = "InAir";
                _figureAnimation.Frame = 1;
                _figureAnimation.Play("InAir");
                Character.StateMachine.TransitionTo(AvailableState.Falling);
            }
            else if (Input.IsActionJustPressed("jump"))
            {
                _figureAnimation.Stop();
                _figureAnimation.Play("JumpingOffGround");
                Character.StateMachine.TransitionTo(AvailableState.Jumping);
            }
            else if (Mathf.IsEqualApprox(inputDirection, 0.0))
            {
                _figureAnimation.Stop();
                Character.StateMachine.TransitionTo(AvailableState.Idle);
            }
            /*else if (_gettingHit)
            {
                _figureAnimation.Stop();
                Character.StateMachine.TransitionTo(AvailableState.TakeingDamage);
            }
            else if (Healthpoints <= 0)
            {
                _figureAnimation.Stop();
                Character.StateMachine.TransitionTo(AvailableState.Dead);
            }*/
        }
    }
}