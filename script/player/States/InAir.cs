public class InAirState : PlayerState
{
    public override void Enter(Player player)
    {
        base.Enter(player);
        // Sprunglogik und andere InAir-spezifische Sachen
    }

    public override void Update(double delta)
    {
        // Logik für den Zustand "in der Luft"
        if (Spieler.IsOnFloor()) // Method aus Godot für das Prüfen, ob der Charakter den Boden berührt
        {
            Spieler.StateMachine.TransitionTo("idle");
        }
    }
}
