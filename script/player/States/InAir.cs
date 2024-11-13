using Godot;

public partial class Inair : State
{
    public override void Enter(Player player)
    {
        base.Enter(player);
        GD.Print("Entering inair State");        //Debugger-Code
    }

    public override void Update(double delta)
    {
        //Spieler in Idle-Zustand versetzen, wenn Boden erreicht
        if (Spieler.IsOnFloor())
        {
            Spieler.StateMachine.TransitionTo("idle");
        }
        else
        {
            Spieler.Velocity += new Vector2(0, Gravity * (float)delta);     //Schwerkraft anwenden
            _ = Spieler.MoveAndSlide();                                     //Bewegung aktualisieren
        }
    }
}
