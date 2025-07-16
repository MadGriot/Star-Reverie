
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
        private MouseWorld mouseWorld;
        private AnimationController AnimationController;
        private CharacterComponent characterComponent;
        public override void Start()
        {
            mouseWorld = Entity.Get<MouseWorld>();
            AnimationController = Entity.Get<AnimationController>();
            characterComponent = Entity.Get<CharacterComponent>();
            targetPosition = Entity.Transform.Position;
            ActionSystem.OnSelectedActorChanged += ActorActionSytem_OnSelectedActorChanged;
        }

        public override void Update()
        {
            if (CurrentGameState.GameState != GameState.Encounter)
                return;
            if (!Actor.Get<Actor>().actorSelected)
            {
                AnimationController.StopMovement();
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
                AnimationController.StopMovement();
            }
            if (Input.IsMouseButtonPressed(MouseButton.Left))
            {
                if (Actor.Get<Actor>().actorSelected)
                    Move(mouseWorld.GetPosition());

            }


        }

        public void Move(Vector3 targetPosition)
        {
            this.targetPosition = targetPosition;
            AnimationController.animationState = AnimationState.Running;
            AnimationController.PlayAnimation(AnimationState.Running);
        }

        public void ResetTarget()
        {
            targetPosition = Entity.Transform.Position;
            AnimationController.StopMovement();
        }

        private void ActorActionSytem_OnSelectedActorChanged(object sender, EventArgs empty)
        {
            ResetTarget();
        }
    }
}
