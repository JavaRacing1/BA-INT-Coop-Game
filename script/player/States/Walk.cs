using Godot;

namespace INTOnlineCoop.Script.Player.States
{
    /// <summary>
    /// State used when the character is walking
    /// </summary>
    public partial class Walk : State
    {
        /// <summary>
        /// Updates player movement
        /// </summary>
        /// <param name="delta">Current frame delta</param>
        public override void PhysicProcess(double delta)
        {
            Vector2 velocity = Character.Velocity;

            //Spieler in den State InAir versetzen falls eine der Bedingungen erfüllt
            if (!Character.IsOnFloor() || Input.IsActionJustPressed("jump"))
            {
                Character.StateMachine.TransitionTo(AvailableState.InAir);
                return;
            }

            //Bewegung nach links
            if (Input.IsActionPressed("walk_left"))
            {
                GD.Print("Move Left");
                velocity.X = -Speed;            //Bewegung entgegen der x-Achse
                Character.Velocity = velocity;  //Charakter die Geschwindigkeit/Richtung übergeben
                _ = Character.MoveAndSlide();   //Bewegung aktualisieren
            }
            //Bewegung nach rechts
            else if (Input.IsActionPressed("walk_right"))
            {
                GD.Print("Move Right");
                velocity.X = Speed;             //Bewegung entlang der x-Achse
                Character.Velocity = velocity;  //Charakter die Geschwindigkeit/Richtung übergeben
                _ = Character.MoveAndSlide();   //Bewegung aktualisieren
            }
            //Wechsel in den State idle, falls keine Bewegung mehr durchgeführt wird
            else
            {
                Character.StateMachine.TransitionTo(AvailableState.Idle);
            }
        }
    }
}