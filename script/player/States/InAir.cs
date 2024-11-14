using Godot;

namespace INTOnlineCoop.Script.Player.States
{
    /// <summary>
    /// State used when player is in the air
    /// </summary>
    public partial class InAir : State
    {
        private const float JumpVelocity = -100f;

        /// <summary>
        /// Updates player movement
        /// </summary>
        /// <param name="delta">Current frame delta</param>
        public override void PhysicProcess(double delta)
        {
            Vector2 velocity = Character.Velocity;

            //Spieler in Idle-Zustand versetzen, wenn Boden erreicht
            if (Character.IsOnFloor())
            {
                Character.StateMachine.TransitionTo(AvailableState.Idle);
                return;
            }
            else if (Input.IsActionJustPressed("jump"))
            {
                if (Input.IsActionPressed("walk_left"))
                {
                    velocity.X = -Speed;            //Bewegung entgegen der x-Achse
                    velocity.Y = JumpVelocity;
                }
                else if (Input.IsActionPressed("walk_right"))
                {
                    velocity.X = Speed;             //Bewegung entlang der x-Achse
                    velocity.Y = JumpVelocity;
                }
            }
            else
            {
                velocity.X = 0;
            }

            velocity.Y += Gravity * (float)delta;
            Character.Velocity = velocity;
            _ = Character.MoveAndSlide();
        }
    }
}

