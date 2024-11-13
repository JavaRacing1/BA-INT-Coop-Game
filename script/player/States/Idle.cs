using Godot;

public partial class Idle : State
{
    public override void Enter(Player player)
    {
        base.Enter(player);
        GD.Print("Entering idle State");        //Debugger-Code
    }

    public override void Update(double delta)
    {
        //Übergang in den Walking-Zustand, falls Eingabe erfolgt
        if (Input.IsActionPressed("walk_right") || Input.IsActionPressed("walk_left"))
        {
            Spieler.StateMachine.TransitionTo("walk");
        }
        //Übergang in den Inair-Zustand, falls in der Luft
        else if (!Spieler.IsOnFloor())
        {
            Spieler.StateMachine.TransitionTo("inair");
        }
    }
}
