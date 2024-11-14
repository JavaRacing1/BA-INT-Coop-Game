using Godot;

namespace INTOnlineCoop.Script.Player.States
{
    /// <summary>
    /// State used when player is in the air
    /// </summary>
    public partial class InAir : State
    {
        /// <summary>
        /// Updates player movement
        /// </summary>
        /// <param name="delta">Current frame delta</param>
        public override void PhysicProcess(double delta)
        {
            //Spieler in Idle-Zustand versetzen, wenn Boden erreicht
            if (Character.IsOnFloor())
            {
                Character.StateMachine.TransitionTo(AvailableState.Idle);
            }
            else
            {
                Character.Velocity += new Vector2(0, Gravity * (float)delta);     //Schwerkraft anwenden
                _ = Character.MoveAndSlide();                                     //Bewegung aktualisieren
            }
        }
    }
}

