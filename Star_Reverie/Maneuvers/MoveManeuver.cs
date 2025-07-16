
using Star_Reverie.Globals;
using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Input;
using Stride.Physics;
using System;
using System.Windows.Automation.Text;

namespace Star_Reverie.Maneuvers
{
    public class MoveManeuver : BaseManeuver
    {
        private Vector3 targetPosition;
        private Vector3 currentPosition;
        public MouseWorld MouseWorld;
        public AnimationState animationState;
        public float AnimationSpeed = 1.0f;
        private AnimationComponent animationComponent;
        private CharacterComponent characterComponent;
        public override void Start()
        {
            characterComponent = Entity.Get<CharacterComponent>();
            animationComponent = Entity.GetChild(1).Get<AnimationComponent>();
            targetPosition = Entity.Transform.Position;
        }

        public override void Update()
        {
            if (CurrentGameState.GameState != GameState.Encounter)
                return;
            if (!Actor.Get<Actor>().actorSelected)
            {
                StopMovement();
                currentPosition = Entity.Transform.Position;
                return;
            }
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
                if (Actor.Get<Actor>().actorSelected)
                    Move(MouseWorld.GetPosition());

            }


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

        public void ResetTarget()
        {
            targetPosition = Entity.Transform.Position;
            StopMovement();
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
