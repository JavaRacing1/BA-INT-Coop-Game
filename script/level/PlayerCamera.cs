using System;

using Godot;

namespace INTOnlineCoop.Script.Level
{
    /// <summary>
    /// Camera of the player
    /// </summary>
    public partial class PlayerCamera : Camera2D
    {
        [Export(PropertyHint.Range, "0,500,")] private int _cameraSpeed = 10;
        [ExportGroup("Zoom")]
        [Export(PropertyHint.Range, "0,1,")] private float _zoomSize = 0.1f;
        [Export(PropertyHint.Range, "0.01,2,")] private float _minZoom = 0.1f;
        [Export(PropertyHint.Range, "0.01,2,")] private float _maxZoom = 2f;

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

            Position += moveVector * _cameraSpeed;
        }

        /// <summary>
        /// Called when an InputEvent occurs
        /// </summary>
        /// <param name="event">The input event</param>
        public override void _UnhandledInput(InputEvent @event)
        {
            if (@event is InputEventMouseButton mouseEvent)
            {
                int zoomDirectionMultiplier = mouseEvent.ButtonIndex switch
                {
                    MouseButton.WheelUp => 1,
                    MouseButton.WheelDown => -1,
                    _ => 0
                };

                if (zoomDirectionMultiplier != 0)
                {
                    float newZoom = Math.Clamp(Zoom.X + (_zoomSize * zoomDirectionMultiplier), _minZoom, _maxZoom);
                    Zoom = new Vector2(newZoom, newZoom);
                }
            }
        }
    }
}