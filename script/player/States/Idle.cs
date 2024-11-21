using Godot;

using INTOnlineCoop.Script.Level;

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
            StateMachine.Jumped = false;
            StateMachine.Direction = 0;
        }

        /// <summary>
        /// Handles input in Idle state
        /// </summary>
        /// <param name="delta">Current frame delta</param>
        public override void HandleInput(double delta)
        {
            if (GameLevel.IsInputBlocked || Character.IsBlocked || Character.PeerId != Multiplayer.GetUniqueId())
            {
                return;
            }

            StateMachine.Jumped = Input.IsActionJustPressed("jump");

            StateMachine.Direction = Input.GetAxis("walk_left", "walk_right");
        }

        /// <summary>
        /// Manages the used animations + state changes
        /// </summary>
        /// <param name="delta">Frame delta</param>
        public override void ChangeAnimationsAndStates(double delta)
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
            else if (StateMachine.Jumped)
            {
                CharacterSprite.Stop();
                CharacterSprite.Play("JumpingOffGround");
                Character.StateMachine.TransitionTo(AvailableState.Jumping);
            }
            else if (!Mathf.IsEqualApprox(StateMachine.Direction, 0.0))
            {
                CharacterSprite.Stop();
                CharacterSprite.FlipH = StateMachine.Direction < 0;
                CharacterSprite.Play("Walking");
                Character.StateMachine.TransitionTo(AvailableState.Walking);
            }
        }
    }
}