using Godot;

public partial class Inactive : State
{
    public override void PhysicProcess(double delta)
    {
        //Wenn Spieler in der Luft ist, fällt er auf den Boden
        Character.Velocity += new Vector2(0, Gravity * (float)delta); //Schwerkraft anwenden
        _ = Character.MoveAndSlide();                                 //Bewegung aktualisieren
    }
}
