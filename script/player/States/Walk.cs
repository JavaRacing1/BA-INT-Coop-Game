using Godot;

public class WalkingState : PlayerState
{
    public override void Enter(PlayerCharacter player)
    {
        base.Enter(player);
        GD.Print("Entering Walking State");
    }

    public override void Update(float delta)
    {
        // Logik für den Walking-Zustand
        // Bewegung und Animationen für das Gehen

        if (!Input.IsActionPressed("move_right") && !Input.IsActionPressed("move_left"))
        {
            player.StateMachine.TransitionTo("idle");
        }

        // Beispiel für Übergang zu InAir bei Sprung
        if (Input.IsActionJustPressed("jump"))
        {
            player.StateMachine.TransitionTo("inair");
        }
    }
}

