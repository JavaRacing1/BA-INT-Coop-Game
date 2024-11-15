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

            if (!Character.IsOnFloor())
            {
                Character.StateMachine.TransitionTo(AvailableState.Falling);
            }
            else if (Input.IsActionJustPressed("jump"))
            {
                Character.StateMachine.TransitionTo(AvailableState.Jumping);
            }

            //Bewegung nach links
            if (Input.IsActionPressed("walk_left"))
            {
                GD.Print("Move Left");
                velocity.X = -Speed; //Bewegung entgegen der x-Achse
                Character.Velocity = velocity; //Charakter die Geschwindigkeit/Richtung übergeben
                _ = Character.MoveAndSlide(); //Bewegung aktualisieren
            }
            //Bewegung nach rechts
            else if (Input.IsActionPressed("walk_right"))
            {
                GD.Print("Move Right");
                velocity.X = Speed; //Bewegung entlang der x-Achse
                Character.Velocity = velocity; //Charakter die Geschwindigkeit/Richtung übergeben
                _ = Character.MoveAndSlide(); //Bewegung aktualisieren
            }
            //Wechsel in den State idle, falls keine Bewegung mehr durchgeführt wird
            else
            {
                Character.StateMachine.TransitionTo(AvailableState.Idle);
            }
        }
    }
}