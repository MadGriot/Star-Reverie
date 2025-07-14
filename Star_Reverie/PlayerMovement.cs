using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stride.Core.Mathematics;
using Stride.Input;
using Stride.Engine;
using Star_Reverie.Globals;
using Stride.Animations;
using Stride.Physics;

namespace Star_Reverie
{
    public class PlayerMovement : SyncScript
    {

        public float AnimationSpeed = 1.0f;
        private AnimationComponent animationComponent;
        public float DeadZone = 0.25f;
        public float MoveSpeed = 1.0f;
        public bool actorSelected;
        private AnimationState animationState;
        private AnimationState animationMovementState;
        private Entity cameraEntity;

        public override void Start()
        {
            animationComponent = Entity.GetChild(1).Get<AnimationComponent>();
            cameraEntity = Entity.GetChild(0);
            animationComponent.Play("Idle");
            animationMovementState = AnimationState.Walking;
            animationState = AnimationState.Idle;
        }

        public override void Update()
        {
            if (!actorSelected)
                return;
            if (Input.IsKeyPressed(Keys.Escape)) // Replace Start button for pause
            {
                CurrentGameState.GameState = (CurrentGameState.GameState == GameState.Paused)
                    ? GameState.Exploration : GameState.Paused;
                StopMovement();
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
                    animationMovementState = isRunning ? AnimationState.Running : AnimationState.Walking;
                    MoveSpeed = isRunning ? 5f : 1.0f;

                    Move(moveInput);

                    if (animationState != animationMovementState)
                    {
                        animationState = animationMovementState;
                        PlayAnimation(animationState);
                    }
                }
                else
                {
                    // No input — stop
                    if (animationState != AnimationState.Idle)
                    {
                        animationState = AnimationState.Idle;
                        PlayAnimation(animationState);
                    }

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

        private void StopMovement()
        {
            Entity.Get<CharacterComponent>().SetVelocity(Vector3.Zero);
            if (animationState != AnimationState.Idle)
            {
                animationState = AnimationState.Idle;
                animationComponent.Play("Idle");
            }
        }
        private void PlayAnimation(AnimationState animationState)
        {
            switch (animationState)
            {
                case AnimationState.Idle:
                    animationComponent.Play("Idle");
                    break;
                case AnimationState.Running:
                    animationComponent.Play("Running");
                    break;
                case AnimationState.Walking:
                    animationComponent.Play("Walking");
                    break;
            }
        }
    }
}
