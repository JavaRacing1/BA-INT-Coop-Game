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
            //Übergang in den Walking-Zustand, falls Eingabe erfolgt
            if (Input.IsActionPressed("walk_right") || Input.IsActionPressed("walk_left"))
            {
                Character.StateMachine.TransitionTo(AvailableState.Walk);
            }
            //Übergang in den Inair-Zustand, falls in der Luft oder Sprung
            else if (!Character.IsOnFloor() || Input.IsActionJustPressed("jump"))
            {
                Character.StateMachine.TransitionTo(AvailableState.InAir);
            }
        }
    }
}