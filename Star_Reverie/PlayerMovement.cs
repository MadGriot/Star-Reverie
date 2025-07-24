using Star_Reverie.Globals;
using Stride.Animations;
using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Input;
using Stride.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Star_Reverie
{
    public class PlayerMovement : SyncScript
    {
        public float DeadZone = 0.25f;
        public float MoveSpeedMultiplier = 1.0f;
        private float MoveSpeed;
        private Entity cameraEntity;
        private AnimationController AnimationController;
        public override void Start()
        {
            AnimationController = Entity.Get<AnimationController>();
            cameraEntity = Entity.GetChild(0);
            CameraComponent camera = cameraEntity.GetChild(0).Get<CameraComponent>();
            if (camera.Slot != SceneSystem.GraphicsCompositor.Cameras[0].ToSlotId())
            {
                camera.Slot = SceneSystem.GraphicsCompositor.Cameras[0].ToSlotId();
                camera.Enabled = false;
            }
        }

        public override void Update()
        {
            if (!Entity.Get<Actor>().actorSelected)
                return;
            if (Input.IsKeyPressed(Keys.Escape)) // Replace Start button for pause
            {
                CurrentGameState.GameState = (CurrentGameState.GameState == GameState.Paused)
                    ? GameState.Exploration : GameState.Paused;
                AnimationController.StopMovement();
            }

            if (CurrentGameState.GameState == GameState.Paused)
                return;

            if (CurrentGameState.GameState == GameState.Exploration)
            {
                // Get input direction from keyboard
                Vector2 moveInput = Vector2.Zero;

                if (Input.IsKeyDown(Keys.W)) moveInput.Y += 1;
                if (Input.IsKeyDown(Keys.S)) moveInput.Y -= 1;
                if (Input.IsKeyDown(Keys.D)) moveInput.X += 1;
                if (Input.IsKeyDown(Keys.A)) moveInput.X -= 1;

                if (moveInput.LengthSquared() > 0)
                {
                    // Normalize input direction
                    moveInput.Normalize();

                    // Shift = run, otherwise walk
                    bool isRunning = Input.IsKeyDown(Keys.LeftShift) || Input.IsKeyDown(Keys.RightShift);
                    AnimationController.animationMovementState = isRunning ? AnimationState.Running : AnimationState.Walking;
                    MoveSpeed = isRunning ? 3f * MoveSpeedMultiplier : MoveSpeedMultiplier;

                    Move(moveInput);
                }
                else
                {
                    // No input — stop
                    AnimationController.animationMovementState = AnimationState.Idle;
                    Entity.Get<CharacterComponent>().SetVelocity(Vector3.Zero);
                }
            }
        }
        private void Move(Vector2 moveDirection)
        {

            CharacterComponent character = Entity.Get<CharacterComponent>();

            Vector3 direction = new Vector3(-moveDirection.X, 0, -moveDirection.Y);
            if (direction.LengthSquared() > 0)
                direction.Normalize();

            // Get camera's forward and right vectors on the XZ plane
            Vector3 cameraForward = cameraEntity.Transform.WorldMatrix.Forward;
            cameraForward.Y = 0;
            cameraForward.Normalize();

            Vector3 cameraRight = cameraEntity.Transform.WorldMatrix.Right;
            cameraRight.Y = 0;
            cameraRight.Normalize();

            // Combine input with camera orientation
            Vector3 cameraDirection = (cameraRight * direction.X + cameraForward * direction.Z);
            cameraDirection.Normalize();

            Vector3 velocity = cameraDirection * MoveSpeed;

            character.SetVelocity(velocity);

            // Compute yaw (Y-axis rotation) from direction vector
            float targetYaw = (float)Math.Atan2(cameraDirection.X, cameraDirection.Z); // returns radians


            Entity.GetChild(1).Transform.RotationEulerXYZ = new Vector3(0, targetYaw, 0);

        }
    }
}
