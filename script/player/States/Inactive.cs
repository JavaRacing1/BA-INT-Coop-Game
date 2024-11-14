using Godot;

public partial class Inactive : State
{
    public override void Enter(PlayerCharacter player)
    {
        base.Enter(player);
        GD.Print("Entering inactive State");        //Debugger-Code
        // Logik für den inaktiven Zustand
    }
    public override void Update(double delta)
    {
        //Wenn Spieler in der Luft ist, fällt er auf den Boden
        Character.Velocity += new Vector2(0, Gravity * (float)delta); //Schwerkraft anwenden
        _ = Character.MoveAndSlide();                                 //Bewegung aktualisieren
    }
}
