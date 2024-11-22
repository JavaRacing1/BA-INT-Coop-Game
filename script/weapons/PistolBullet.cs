using Godot;

namespace INTOnlineCoop.Script.Weapons
{
    /// <summary>
    /// Beschreibt das Verhalten einer Pistolenkugel, wenn der Spieler die Pistole abfeuert
    /// </summary>
    public partial class PistolBullet : RigidBody2D
    {
        /// <summary>
        /// Maximale Distanz, die das Projektil fliegen kann
        /// </summary>
        public const float MaxDistance = 600;

        /// <summary>
        /// Maximaler Impuls ("Bewegungskraft"), mit der sich das Projektil bewegt
        /// </summary>
        public const float Impulse = 1200;

        /// <summary>
        /// Maximale Zeit, die das Projektil in der Szene existiert
        /// </summary>
        public const float Life = 1;
        //Position des aktiven Spielers (plus Offset auf Spritemitte)
        private Vector2 _originalPistolPosition;
        //Position/ Winkel zum Abschießen der Waffe (Endpunkt für Vector2 Linie)
        private Vector2 _shootDirection = Vector2.Right;
        //Timer für "Leben" des Projektils
        private Timer _timerPistolBullet;

        /// <summary>
        /// Initialisiert das Projektil
        /// </summary>
        public override void _Ready()
        {
            _timerPistolBullet = new Timer
            {
                WaitTime = Life,
                OneShot = true
            };
            _timerPistolBullet.Timeout += OnTimeToDie;
            AddChild(_timerPistolBullet);
        }

        /// <summary>
        /// Überprüft Eingaben und feuert die Pistole, wenn alle Bedingungen erfüllt sind
        /// </summary>
        public override void _UnhandledInput(InputEvent @event)
        {
            if (!IsPistolSelected())
            {
                return;
            }

            if (Input.IsActionPressed("aim_up")) // Aim up
            {
                _shootDirection = Vector2.Up;
            }
            else if (Input.IsActionPressed("aim_down")) // Aim down
            {
                _shootDirection = Vector2.Down;
            }
            else if (Input.IsActionPressed("use_item")) // Fire
            {
                GD.Print("Pistole abgeschossen");
                LaunchBullet();
            }

        }

        /// <summary>
        /// Prüft, ob die Pistole als Waffe ausgewählt wurde
        /// </summary>
        private bool IsPistolSelected()
        {
            // Hier sollte die Logik implementiert werden, die prüft, ob die Pistole ausgewählt ist
            return true; // Platzhalter
        }

        /// <summary>
        /// Spawnt das Projektil an der Position des Spielers und schießt es ab
        /// ------
        /// noch Spielerposition für _originalPosition hinzufügen
        /// </summary>
        public void LaunchBullet()
        {
            //ein Schuss in Richtung von Position Character (Position)
            //nach _shootDirection abfeuern
            /*_originalPistolPosition = Position;
            ApplyCentralImpulse(_shootDirection.Normalized() * Impulse);*/

            // Starte den Timer
            _timerPistolBullet.Start();
        }
        /// <summary>
        /// überprüfe jeden Frame, ob Projektil seine Maximale Distanz geflogen ist, 
        /// sein Timer abgelaufen ist, oder er mit einen anderen CollisionShape zusammengestoßen ist
        /// </summary>
        /// <param name="delta"></param>
        public override void _PhysicsProcess(double delta)
        {
            // Überprüfen der maximalen Flugdistanz
            // Distance = aktuelle Vector2 Position - _originalPistolPosition
            float distanceTravelled = Position.DistanceTo(_originalPistolPosition);
            if (distanceTravelled > MaxDistance)
            {
                QueueFree();
                return;
            }

            // Kollision mit einer anderen Figur prüfen
            if (GetCollidingBodies().Count > 0)
            {
                QueueFree();
                // Code für Schadenberechnug auslösen wenn
                // Kollision mit einen anderen Spielerobjekt
            }
        }

        /// <summary>
        /// Entfernt das Projektil nach Ablauf der Zeit
        /// </summary>
        private void OnTimeToDie()
        {
            QueueFree();
        }
    }
}
