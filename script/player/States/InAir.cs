using Godot;

public class InAirState : PlayerState
{
    public override void Enter(PlayerCharacter player)
    {
        base.Enter(player);
        GD.Print("Entering InAir State");
        // Sprunglogik und andere InAir-spezifische Sachen
    }

    public override void Update(float delta)
    {
        // Logik für den Zustand "in der Luft"
        if (player.IsOnFloor()) // Method aus Godot für das Prüfen, ob der Charakter den Boden berührt
        {
            player.StateMachine.TransitionTo("idle");
        }
    }
}

