using Godot;

public partial class Inair : PlayerState
{
    public override void Enter(Player player)
    {
        base.Enter(player);
        GD.Print("Entering InAir State");
    }

    public override void Update(double delta)
    {
        // Wenn der Spieler auf dem Boden ist, wechsle in den Idle-Status
        if (Spieler.IsOnFloor())
        {
            Spieler.StateMachine.TransitionTo("idle");
            return;
        }

        // Schwerkraft anwenden
        Spieler.Velocity += new Vector2(0, Gravity * (float)delta);

        // Bewegung aktualisieren
        _ = Spieler.MoveAndSlide();
    }
}
