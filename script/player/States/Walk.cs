using Godot;

public partial class Walk : PlayerState
{
    public override void Enter(Player player)
    {
        base.Enter(player);
        GD.Print("Entering walk State");
        // Hier könnten Animationen oder andere zustandsspezifische Logik stehen
    }

    public override void Update(double delta)
    {
        Vector2 velocity = Spieler.Velocity;

        // Bewegung nach links
        if (Input.IsActionPressed("walk_left"))
        {
            GD.Print("Move Left");
            velocity.X = -Speed;
            //Spieler.FlipH = true; // Spieler nach links drehen
            _ = Spieler.MoveAndSlide();
        }
        // Bewegung nach rechts
        else if (Input.IsActionPressed("walk_right"))
        {
            GD.Print("Move Right");
            velocity.X = Speed;
            //Spieler.FlipH = false; // Spieler nach rechts drehen
            _ = Spieler.MoveAndSlide();
        }
        else if (!Input.IsActionPressed("walk_right") && !Input.IsActionPressed("walk_left"))
        {
            Spieler.StateMachine.TransitionTo("idle");
        }
        else if (Input.IsActionJustPressed("jump") || !Spieler.IsOnFloor())
        {
            Spieler.StateMachine.TransitionTo("inair");
        }
    }
}
