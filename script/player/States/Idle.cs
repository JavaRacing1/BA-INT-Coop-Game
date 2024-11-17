using Godot;

namespace INTOnlineCoop.Script.Player.States
{
    /// <summary>
    /// State used when the character is not moving
    /// </summary>
    public partial class Idle : State
    {
        /// <summary>
        /// reference to the usage of the AnimationPlayer build into AnimatedSprite2D with his SpriteFrames and Animation options
        /// </summary>
        [Export] private AnimatedSprite2D _figureAnimation;

        // private bool _gettingHit;

        /// <summary>
        /// define what animation is played when entering the state
        /// </summary>
        public override void Enter()
        {
            base.Enter();
            _figureAnimation.Play("Idle");
        }

        /// <summary>
        /// Updates player movement and changeing played animation
        /// </summary>
        /// <param name="delta">Current frame delta</param>
        public override void PhysicProcess(double delta)
        {
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
            //Übergang in den Walking-Zustand, falls Eingabe erfolgt
            else if (Input.IsActionPressed("walk_right") || Input.IsActionPressed("walk_left"))
            {
                _figureAnimation.Stop();
                if (Input.IsActionPressed("walk_right"))
                {
                    _figureAnimation.FlipH = false;
                    _figureAnimation.Play("Walking");
                }
                else
                {
                    _figureAnimation.FlipH = true;
                    _figureAnimation.Play("Walking");
                }
                Character.StateMachine.TransitionTo(AvailableState.Walking);
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