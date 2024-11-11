using Godot;

public class WalkingState : PlayerState
{
    public override void Enter(Player player)
    {
        base.Enter(player);
    }

    public override void Update(double delta)
    {
        // Logik für den Walking-Zustand
        // Bewegung und Animationen für das Gehen

        if (!Input.IsActionPressed("move_right") && !Input.IsActionPressed("move_left"))
        {
            Spieler.StateMachine.TransitionTo("idle");
        }
    }
}
