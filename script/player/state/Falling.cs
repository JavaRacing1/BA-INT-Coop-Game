using Godot;

using INTOnlineCoop.Script.Level;

namespace INTOnlineCoop.Script.Player.States
{
    /// <summary>
    /// State used when player is falling
    /// </summary>
    public partial class Falling : State
    {

        /// <summary>
        /// Handles falling input
        /// </summary>
        /// <param name="delta"></param>
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

            if (Input.IsActionJustPressed("jump") && !StateMachine.HasDoubleJumped)
            {
                Error error = StateMachine.Rpc(StateMachine.MethodName.Jump);
                if (error != Error.Ok)
                {
                    GD.PrintErr($"Error during Jump RPC: {error}");
                }

                StateMachine.HasDoubleJumped = true;
            }

            StateMachine.Direction = Input.GetAxis("walk_left", "walk_right");
        }

        /// <summary>
        /// Changes falling animations + states
        /// </summary>
        /// <param name="delta"></param>
        public override void ChangeAnimationsAndStates(double delta)
        {
            if ((CharacterSprite.Animation == "JumpingOffGround" || CharacterSprite.Animation == "InAir") &&
                Character.IsOnFloor())
            {
                //InAir Animation muss beendet werden, da Kollision mit Boden erkannt
                //-> wechsel auf LandingOnGround Animation
                CharacterSprite.Stop();
                CharacterSprite.Play("LandingOnGround");
            }

            if (!Mathf.IsEqualApprox(StateMachine.Direction, 0))
            {
                CharacterSprite.FlipH = StateMachine.Direction < 0;
                Character.UpdateWeaponDirection();
            }

            if (StateMachine.Jumped)
            {
                CharacterSprite.Stop();
                CharacterSprite.Play("JumpingOffGround");
                Character.StateMachine.TransitionTo(AvailableState.Jumping);
            }

            if (!Character.IsOnFloor())
            {
                return;
            }

            if ((CharacterSprite.Animation == "LandingOnGround") && (CharacterSprite.Frame == 2))
            {
                Character.StateMachine.TransitionTo(Mathf.IsEqualApprox(StateMachine.Direction, 0.0)
                    ? AvailableState.Idle
                    : AvailableState.Walking);
            }
        }

        /// <summary>
        /// Updates player movement
        /// </summary>
        /// <param name="delta">Current frame delta</param>
        public override void PhysicProcess(double delta)
        {
            Vector2 velocity = Character.Velocity;

            velocity.X = StateMachine.Direction * Speed;
            velocity.Y += Gravity * (float)delta;
            Character.Velocity = velocity;
            _ = Character.MoveAndSlide();
        }
    }
}