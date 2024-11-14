using Godot;

public partial class InAir : State
{
    public override void Enter(PlayerCharacter player)
    {
        base.Enter(player);
        GD.Print("Entering inair State");        //Debugger-Code
    }

    public override void Update(double delta)
    {
        //Spieler in Idle-Zustand versetzen, wenn Boden erreicht
        if (Character.IsOnFloor())
        {
            Character.StateMachine.TransitionTo("idle");
        }
        else
        {
            Character.Velocity += new Vector2(0, Gravity * (float)delta);     //Schwerkraft anwenden
            _ = Character.MoveAndSlide();                                     //Bewegung aktualisieren
        }
    }
}
