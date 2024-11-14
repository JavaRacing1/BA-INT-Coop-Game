using Godot;

namespace INTOnlineCoop.Script.Player.States
{
    /// <summary>
    /// State used when the character is not active
    /// </summary>
    public partial class Inactive : State
    {
        /// <summary>
        /// Updates player movement
        /// </summary>
        /// <param name="delta">Current frame delta</param>
        public override void PhysicProcess(double delta)
        {
            //Wenn Spieler in der Luft ist, fällt er auf den Boden
            Character.Velocity += new Vector2(0, Gravity * (float)delta); //Schwerkraft anwenden
            _ = Character.MoveAndSlide();                                 //Bewegung aktualisieren
        }
    }
}