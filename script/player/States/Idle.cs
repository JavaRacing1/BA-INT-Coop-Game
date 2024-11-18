using Godot;

namespace INTOnlineCoop.Script.Player.States
{
    /// <summary>
    /// State used when the character is not moving
    /// </summary>
    public partial class Idle : State
    {
        // private bool _gettingHit;
        private int _idleFrameCounter;

        /// <summary>
        /// define what animation is played when entering the state
        /// </summary>
        public override void Enter()
        {
            base.Enter();
            CharacterSprite.Animation = "Idle";
            CharacterSprite.Pause();
        }

        /// <summary>
        /// Updates player movement and changeing played animation
        /// </summary>
        /// <param name="delta">Current frame delta</param>
        public override void PhysicProcess(double delta)
        {
            _idleFrameCounter++;
            if (_idleFrameCounter == 0)
            {
                CharacterSprite.Frame = 0;
            }
            else if (CharacterSprite.Frame == Character.Type.LastIdleFrame && _idleFrameCounter > 0)
            {
                CharacterSprite.Pause();
                _idleFrameCounter = -20;
            }
            else if (_idleFrameCounter == 400)
            {
                CharacterSprite.Play();
            }

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
            //Übergang in den Walking-Zustand, falls Eingabe erfolgt
            else if (Input.IsActionPressed("walk_right") || Input.IsActionPressed("walk_left"))
            {
                CharacterSprite.Stop();
                if (Input.IsActionPressed("walk_right"))
                {
                    CharacterSprite.FlipH = false;
                    CharacterSprite.Play("Walking");
                }
                else
                {
                    CharacterSprite.FlipH = true;
                    CharacterSprite.Play("Walking");
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