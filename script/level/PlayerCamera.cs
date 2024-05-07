using Godot;

namespace INTOnlineCoop.Script.Level
{
    /// <summary>
    /// Camera of the player
    /// </summary>
    public partial class PlayerCamera : Camera2D
    {
        private const int CameraSpeed = 10;

        /// <summary>
        /// Called every frame
        /// </summary>
        public override void _Process(double delta)
        {
            Vector2I moveVector = Vector2I.Zero;
            if (Input.IsActionPressed("camera_left"))
            {
                moveVector.X -= 1;
            }

            if (Input.IsActionPressed("camera_up"))
            {
                moveVector.Y -= 1;
            }

            if (Input.IsActionPressed("camera_right"))
            {
                moveVector.X += 1;
            }

            if (Input.IsActionPressed("camera_down"))
            {
                moveVector.Y += 1;
            }

            Position += moveVector * CameraSpeed;
        }
    }
}