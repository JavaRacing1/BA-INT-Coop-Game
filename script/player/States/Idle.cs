using Godot;

public class IdleState : PlayerState
{
    public override void Enter(PlayerCharacter player)
    {
        base.Enter(player);
        GD.Print("Entering Idle State");
        // Hier könnten Animationen oder andere Zustands-spezifische Logik hinzugefügt werden
    }

    public override void Update(float delta)
    {
        // Logik für den idle-Zustand, z.B. Übergang zu Walking bei Bewegungseingabe
        if (Input.IsActionPressed("move_right") || Input.IsActionPressed("move_left"))
        {
            player.StateMachine.TransitionTo("walking");
        }
    }
}

