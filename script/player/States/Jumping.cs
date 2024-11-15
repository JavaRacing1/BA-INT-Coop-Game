using Godot;

namespace INTOnlineCoop.Script.Player.States
{
    /// <summary>
    /// State used when the player is jumping
    /// </summary>
    public partial class Jumping : State
    {
        private const float JumpVelocity = 100f;

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
            Vector2 velocity = Character.Velocity;

            if (Input.IsActionPressed("walk_left"))
            {
                velocity.X = -Speed; //Bewegung entgegen der x-Achse
            }
            else if (Input.IsActionPressed("walk_right"))
            {
                velocity.X = Speed; //Bewegung entlang der x-Achse
            }

            velocity.Y += Gravity * (float)delta;
            Character.Velocity = velocity;
            _ = Character.MoveAndSlide();

            if (Character.Velocity.Y > 0)
            {
                Character.StateMachine.TransitionTo(AvailableState.Falling);
            }
        }
    }
}