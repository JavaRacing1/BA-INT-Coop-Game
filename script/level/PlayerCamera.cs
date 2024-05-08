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
        [Export(PropertyHint.Range, "0,200,")] private int _mouseMoveDistance = 20;

        [ExportGroup("Limit")]
        // Maximum pixel offsets of the camera to the terrain, used for calculating the camera limits
        [Export(PropertyHint.Range, "0,10000,")] private int _cameraLimitOffsetX = 300;
        [Export(PropertyHint.Range, "0,10000,")] private int _cameraLimitOffsetY = 120;

        [ExportGroup("Zoom")]
        [Export(PropertyHint.Range, "0,1,")] private float _zoomSize = 0.1f;
        [Export(PropertyHint.Range, "0.01,2,")] private float _minZoom = 0.1f;
        [Export(PropertyHint.Range, "0.01,2,")] private float _maxZoom = 2f;

        /// <summary>
        /// Initializes the camera
        /// </summary>
        /// <param name="terrainSize">Size of the terrain</param>
        public void Init(Vector2I terrainSize)
        {
            LimitLeft = -_cameraLimitOffsetX;
            LimitTop = -_cameraLimitOffsetY * 2;
            LimitRight = terrainSize.X + _cameraLimitOffsetX;
            LimitBottom = terrainSize.Y + _cameraLimitOffsetY;

            Position = new Vector2(terrainSize.X / 2f, terrainSize.Y / 2f);
            Zoom = new Vector2(0.1f, 0.1f);
        }

        /// <summary>
        /// Called every frame
        /// </summary>
        public override void _Process(double delta)
        {
            Vector2 mousePosition = GetViewport().GetMousePosition();
            Vector2I moveVector = Vector2I.Zero;
            if (Input.IsActionPressed("camera_left") ||
                (mousePosition.X <= _mouseMoveDistance && IsMouseInsideWindow()))
            {
                moveVector.X -= 1;
            }

            if (Input.IsActionPressed("camera_up") || (mousePosition.Y <= _mouseMoveDistance && IsMouseInsideWindow()))
            {
                moveVector.Y -= 1;
            }

            if (Input.IsActionPressed("camera_right") ||
                (mousePosition.X >= GetViewportRect().Size.X - _mouseMoveDistance && IsMouseInsideWindow()))
            {
                moveVector.X += 1;
            }

            if (Input.IsActionPressed("camera_down") ||
                (mousePosition.Y >= GetViewportRect().Size.Y - _mouseMoveDistance && IsMouseInsideWindow()))
            {
                moveVector.Y += 1;
            }

            Position += moveVector * (int)(_cameraSpeed * (1 / Zoom.X));
            if (moveVector != Vector2I.Zero)
            {
                PositionSmoothingEnabled = true;
                LimitPosition();
            }
        }

        /// <summary>
        /// Called when an InputEvent occurs
        /// </summary>
        /// <param name="event">The input event</param>
        public override void _UnhandledInput(InputEvent @event)
        {
            if (@event is InputEventMouseButton mouseButtonEvent)
            {
                int zoomDirectionMultiplier = mouseButtonEvent.ButtonIndex switch
                {
                    MouseButton.WheelUp => 1,
                    MouseButton.WheelDown => -1,
                    _ => 0
                };

                if (zoomDirectionMultiplier != 0)
                {
                    PositionSmoothingEnabled = false;
                    float newZoom = Math.Clamp(Zoom.X + (_zoomSize * zoomDirectionMultiplier), _minZoom, _maxZoom);
                    Zoom = new Vector2(newZoom, newZoom);
                }
            }
        }

        private void LimitPosition()
        {
            float halfViewportX = GetViewportRect().Size.X / 2 * (1 / Zoom.X);
            float halfViewportY = GetViewportRect().Size.Y / 2 * (1 / Zoom.Y);
            float limitedX = Math.Clamp(Position.X, LimitLeft + halfViewportX, LimitRight - halfViewportX);
            float limitedY = Math.Clamp(Position.Y, LimitTop + halfViewportY, LimitBottom - halfViewportY);
            Position = new Vector2(limitedX, limitedY);
        }

        private bool IsMouseInsideWindow()
        {
            Vector2 mousePosition = GetViewport().GetMousePosition();
            Vector2 windowSize = GetViewportRect().Size;
            return 0 <= mousePosition.X && mousePosition.X <= windowSize.X && 0 <= mousePosition.Y &&
                   mousePosition.Y <= windowSize.Y;
        }
    }
}