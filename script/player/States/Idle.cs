using Godot;

public partial class Idle : State
{
    public override void Enter(PlayerCharacter player)
    {
        base.Enter(player);
        GD.Print("Entering idle State");        //Debugger-Code
    }

    public override void Update(double delta)
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
