using Godot;

public partial class Idle : State
{
    public override void PhysicProcess(double delta)
    {
        //Übergang in den Walking-Zustand, falls Eingabe erfolgt
        if (Input.IsActionPressed("walk_right") || Input.IsActionPressed("walk_left"))
        {
            Character.StateMachine.TransitionTo("walk");
        }
        //Übergang in den Inair-Zustand, falls in der Luft
        else if (!Character.IsOnFloor())
        {
            Character.StateMachine.TransitionTo("inair");
        }
    }
}
