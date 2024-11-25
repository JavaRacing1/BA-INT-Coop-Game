using Godot;

using INTOnlineCoop.Script.Player;

namespace INTOnlineCoop.Script.Item
{
    /// <summary>
    /// Basic bullet
    /// </summary>
    public partial class Bullet : Area2D
    {
        [Export] private int _damage;
        [Export] private int _speed = 10;

        /// <summary>
        /// Direction of the bullet
        /// </summary>
        public Vector2 Direction { get; set; }

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
            if (Position.X < 0 || Position.Y < 0 || Position.X > 8000 || Position.Y > 5000)
            {
                if (Multiplayer.IsServer())
                {
                    QueueFree();
                }
            }

            Rotation = Direction.Angle();
            Position += Direction * _speed * (float)delta;
        }

        private void OnBodyEntered(Node2D body)
        {
            switch (body)
            {
                case PlayerCharacter { IsBlocked: false }:
                    return;
                case PlayerCharacter character when Multiplayer.IsServer():
                    _ = character.Rpc(PlayerCharacter.MethodName.Damage, _damage);
                    break;
            }

            if (Multiplayer.IsServer())
            {
                QueueFree();
            }
        }
    }
}