using Godot;

public partial class Idle : PlayerState
{
    public override void Enter(Player player)
    {
        base.Enter(player);
        GD.Print("Entering idle State");
    }

    public override void Update(double delta)
    {
        //Übergang in den Walking-Zustand, sobald die nötige Eingabe erfolgt
        if (Input.IsActionPressed("walk_right") || Input.IsActionPressed("walk_left"))
        {
            Spieler.StateMachine.TransitionTo("walk");
        }
        else if (Input.IsActionJustPressed("jump") || !Spieler.IsOnFloor())
        {
            Spieler.StateMachine.TransitionTo("inair");
        }
    }
}
