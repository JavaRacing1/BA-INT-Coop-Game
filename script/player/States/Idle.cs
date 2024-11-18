using Godot;

using INTOnlineCoop.Script.Util;

namespace INTOnlineCoop.Script.Player.States
{
    /// <summary>
    /// State used when the character is not moving
    /// </summary>
    public partial class Idle : State
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
            Jumped = false;
            Direction = 0;
        }

        /// <summary>
        /// Handles input in Idle state
        /// </summary>
        /// <param name="delta">Current frame delta</param>
        public override void HandleInput(double delta)
        {
            if (InputBlocker.IsActionJustPressed(Character, "jump"))
            {
                Jumped = true;
            }
            else
            {
                Direction = InputBlocker.GetAxis(Character, "walk_left", "walk_right");
            }
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
            else if (Jumped)
            {
                CharacterSprite.Stop();
                CharacterSprite.Play("JumpingOffGround");
                Jumped = false;
                Character.StateMachine.TransitionTo(AvailableState.Jumping);
            }
            else if (Mathf.IsEqualApprox(Direction, 0.0))
            {
                CharacterSprite.Stop();
                CharacterSprite.FlipH = Direction < 0;
                CharacterSprite.Play("Walking");
                Direction = 0;
                Character.StateMachine.TransitionTo(AvailableState.Walking);
            }
        }
    }
}