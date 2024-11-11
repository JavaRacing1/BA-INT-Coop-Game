using Godot;

public class IdleState : PlayerState
{
    public override void Enter(Player player)
    {
        base.Enter(player);
        // Hier könnten Animationen oder andere Zustands-spezifische Logik hinzugefügt werden
    }

    public override void Update(double delta)
    {
        // Logik für den idle-Zustand, z.B. Übergang zu Walking bei Bewegungseingabe
        if (Input.IsActionPressed("move_right") || Input.IsActionPressed("move_left"))
        {
            Spieler.StateMachine.TransitionTo("walking");
        }
    }
}
