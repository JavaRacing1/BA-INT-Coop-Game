using Godot;

namespace INTOnlineCoop.Script.Player.States
{
    /// <summary>
    /// State used when the character is not moving
    /// </summary>
    public partial class Idle : State
    {
        /// <summary>
        /// Updates player movement
        /// </summary>
        /// <param name="delta">Current frame delta</param>
        public override void PhysicProcess(double delta)
        {
            if (!Character.IsOnFloor())
            {
                Character.StateMachine.TransitionTo(AvailableState.Falling);
            }
            else if (Input.IsActionJustPressed("jump"))
            {
                Character.StateMachine.TransitionTo(AvailableState.Jumping);
            }
            //Übergang in den Walking-Zustand, falls Eingabe erfolgt
            else if (Input.IsActionPressed("walk_right") || Input.IsActionPressed("walk_left"))
            {
                Character.StateMachine.TransitionTo(AvailableState.Walking);
            }
        }
    }
}