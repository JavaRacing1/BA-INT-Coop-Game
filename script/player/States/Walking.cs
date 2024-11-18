using Godot;

namespace INTOnlineCoop.Script.Player.States
{
    /// <summary>
    /// State used when the character is walking
    /// </summary>
    public partial class Walking : State
    {
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
                CharacterSprite.FlipH = false;
            }
            else if (Input.IsActionPressed("walk_left"))
            {
                CharacterSprite.FlipH = true;
            }

            velocity.X = inputDirection * Speed;

            Character.Velocity = velocity;
            _ = Character.MoveAndSlide();

            if (!Character.IsOnFloor())
            {
                CharacterSprite.Stop();
                CharacterSprite.Animation = "InAir";
                CharacterSprite.Frame = 1;
                CharacterSprite.Play("InAir");
                Character.StateMachine.TransitionTo(AvailableState.Falling);
            }
            else if (Input.IsActionJustPressed("jump"))
            {
                CharacterSprite.Stop();
                CharacterSprite.Play("JumpingOffGround");
                Character.StateMachine.TransitionTo(AvailableState.Jumping);
            }
            else if (Mathf.IsEqualApprox(inputDirection, 0.0))
            {
                CharacterSprite.Stop();
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