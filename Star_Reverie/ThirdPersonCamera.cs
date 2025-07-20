using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stride.Core.Mathematics;
using Stride.Input;
using Stride.Engine;

namespace Star_Reverie
{
    public class ThirdPersonCamera : SyncScript
    {
        public float DeadZone = 0.25f;
        public float CameraSensitivity = 1.5f;

        public float MinVerticalAngle { get; set; } = -20f;
        public float MaxVerticalAngle { get; set; } = 70f;
        private Vector2 rightThumbMovement = Vector2.Zero;
        private Vector3 cameraRotationXYZ;
        private Vector3 targetRotationXYZ;

        public override void Start()
        {
            cameraRotationXYZ = Entity.Transform.RotationEulerXYZ;
            targetRotationXYZ = Entity.GetParent().Transform.RotationEulerXYZ;
        }

        public override void Update()
        {
            float deltaTime = (float)Game.UpdateTime.Elapsed.TotalSeconds;
            float MoveStep = 5;
            if (Input.MouseWheelDelta < 0)
            {
                Entity.Transform.Position.Z -= MoveStep * deltaTime;
                Entity.Transform.Position.Z = Math.Clamp(Entity.Transform.Position.Z, -6f, -2f);
            }
            if (Input.MouseWheelDelta > 0)
            {
                Entity.Transform.Position.Z += MoveStep * deltaTime;
                Entity.Transform.Position.Z = Math.Clamp(Entity.Transform.Position.Z, -6f, -2f);
            }

            if (Input.IsMouseButtonDown(MouseButton.Right))
            {
                rightThumbMovement = Input.MouseDelta * CameraSensitivity;
            }
            else
            {
                rightThumbMovement = Vector2.Zero;
            }
            // --- Zoom-based angle logic ---
            float zoomZ = Entity.Transform.Position.Z; // from -6 (far) to -2 (near)
            float zoomT = MathUtil.Clamp(1f - ((-zoomZ - 2f) / 4f), 0f, 1f);

            // Angle bounds (more range when zoomed in)
            float minAngleFar = -20f;
            float maxAngleFar = 70f;
            float minAngleClose = -10f; // Less downward tilt when zoomed in
            float maxAngleClose = 30f;  // Less upward tilt when zoomed in

            // Interpolate based on zoom
            MinVerticalAngle = MathUtil.Lerp(minAngleFar, minAngleClose, zoomT);
            MaxVerticalAngle = MathUtil.Lerp(maxAngleFar, maxAngleClose, zoomT);

            // --- Apply vertical and horizontal rotation ---
            targetRotationXYZ.X += rightThumbMovement.Y;
            targetRotationXYZ.X = MathUtil.Clamp(
                targetRotationXYZ.X,
                MathUtil.DegreesToRadians(MinVerticalAngle),
                MathUtil.DegreesToRadians(MaxVerticalAngle)
            );


            targetRotationXYZ.Y += -rightThumbMovement.X;
            cameraRotationXYZ = Vector3.Lerp(cameraRotationXYZ, targetRotationXYZ, 0.25f);
            Entity.GetParent().Transform.RotationEulerXYZ = new Vector3(cameraRotationXYZ.X, cameraRotationXYZ.Y, 0);

            DebugText.Print($"Joystick Vector: {rightThumbMovement.ToString()}", new Int2(500, 500));
        }
    }
}
