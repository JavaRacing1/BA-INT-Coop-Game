using Godot;

namespace INTOnlineCoop.Script.Player.States
{
    /// <summary>
    /// State used when player is falling
    /// </summary>
    public partial class Falling : State
    {
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

            //Spieler in Idle-Zustand versetzen, wenn Boden erreicht
            if (Character.IsOnFloor())
            {
                Character.StateMachine.TransitionTo(AvailableState.Idle);
            }
        }
    }
}