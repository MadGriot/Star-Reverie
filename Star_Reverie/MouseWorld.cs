using Star_Reverie.Globals;
using Stride.Animations;
using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Graphics;
using Stride.Input;
using Stride.Physics;
using System;

namespace Star_Reverie
{
    public class MouseWorld : SyncScript
    {
        public float AnimationSpeed = 1.0f;
        private AnimationComponent animationComponent;
        public AnimationState animationState;
        public CameraComponent camera;
        internal Simulation simulation;
        public CollisionFilterGroupFlags CollideWith;
        public bool CollideWithTriggers;
        private Vector3 targetPosition;
        private Vector3 currentPosition;
        public bool actorSelected;
        private CharacterComponent characterComponent;
        public override void Start()
        {
            characterComponent = Entity.Get<CharacterComponent>();
            animationComponent = Entity.GetChild(1).Get<AnimationComponent>();
            simulation = this.GetSimulation();
            if (camera.Slot != SceneSystem.GraphicsCompositor.Cameras[0].ToSlotId())
            {
                camera.Slot = SceneSystem.GraphicsCompositor.Cameras[0].ToSlotId();
                camera.Enabled = false;
            }
            targetPosition = Entity.Transform.Position;

        }

        public override void Update()
        {
            if (CurrentGameState.GameState != GameState.Encounter)
                return;
            if (!actorSelected)
                return;
            DebugText.Print($"{targetPosition}", new Int2(700, 800));

            float stoppingDistance = 0.1f;
            currentPosition = Entity.Transform.Position;
            if (Vector3.Distance(currentPosition, targetPosition) > stoppingDistance)
            {
                Vector3 velocity = Vector3.Normalize(targetPosition - currentPosition);
                float moveSpeed = 6f;
                float deltaTime = (float)Game.UpdateTime.Elapsed.TotalSeconds;
                characterComponent.SetVelocity(velocity * moveSpeed);

                float targetYaw = (float)Math.Atan2(velocity.X, velocity.Z); // returns radians

                Entity.GetChild(1).Transform.RotationEulerXYZ = new Vector3(0, targetYaw, 0);
            }
            else
            {
                StopMovement();
            }
            if (Input.IsMouseButtonPressed(MouseButton.Left))
            {
                if (actorSelected)
                    Move(GetPosition());

            }
        }

        public Vector3 GetPosition()
        {
            Texture backbuffer = GraphicsDevice.Presenter.BackBuffer;
            Viewport viewport = new Viewport(0, 0, backbuffer.Width, backbuffer.Height);

            Vector3 nearPosition = viewport.Unproject(new Vector3(Input.AbsoluteMousePosition, 0),
                camera.ProjectionMatrix, camera.ViewMatrix, Matrix.Identity);

            Vector3 farPosition = viewport.Unproject(new Vector3(Input.AbsoluteMousePosition, 1.0f),
                camera.ProjectionMatrix, camera.ViewMatrix, Matrix.Identity);

            HitResult hitResult = simulation.Raycast(nearPosition, farPosition);

            return hitResult.Point;
        }

        public void Move(Vector3 targetPosition)
        {
            this.targetPosition = targetPosition;
            animationState = AnimationState.Running;
            PlayAnimation(animationState);
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
        public void PlayAnimation(AnimationState animationState)
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
