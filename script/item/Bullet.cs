using Godot;

using INTOnlineCoop.Script.Player;

namespace INTOnlineCoop.Script.Item
{
    /// <summary>
    /// Basic bullet
    /// </summary>
    public partial class Bullet : CharacterBody2D
    {
        [Export] private int _damage;
        [Export] private int _speed = 10;

        /// <summary>
        /// Direction of the bullet
        /// </summary>
        public Vector2 Direction { get; set; }

        /// <summary>
        /// Character who fired the bullet
        /// </summary>
        public PlayerCharacter Shooter { get; set; }

        public override void _Ready()
        {
            SetPhysicsProcess(!Multiplayer.HasMultiplayerPeer() || Multiplayer.GetUniqueId() == 1);
        }

        /// <summary>
        /// Moves the bullet
        /// </summary>
        /// <param name="delta"></param>
        public override void _PhysicsProcess(double delta)
        {
            if (Position.X < 0 || Position.Y < 0 || Position.X > 20000 || Position.Y > 4000)
            {
                QueueFree();
            }

            Rotation = Direction.Angle();
            Vector2 velocity = Velocity;
            velocity += Direction * _speed * (float)delta;
            Velocity = velocity;
            _ = MoveAndSlide();
        }
    }
}