using Star_Reverie.Globals;
using StarReverieCore.Grid;
using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Input;
using Stride.Physics;
using System;
using System.Collections.Generic;

namespace Star_Reverie.Maneuvers
{
    public class MoveManeuver : BaseManeuver
    {
        private Vector3 targetPosition;
        private Vector3 currentPosition;
        private MouseWorld mouseWorld;
        private AnimationController AnimationController;
        private CharacterComponent characterComponent;
        public int MaxMoveDistance = 3;
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
            if (!isActive) return;
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
                AnimationController.animationMovementState = AnimationState.Running;
                Vector3 velocity = Vector3.Normalize(targetPosition - currentPosition);
                float moveSpeed = 6f;
                float deltaTime = (float)Game.UpdateTime.Elapsed.TotalSeconds;
                characterComponent.SetVelocity(velocity * moveSpeed);

                float targetYaw = (float)Math.Atan2(velocity.X, velocity.Z); // returns radians

                Entity.GetChild(1).Transform.RotationEulerXYZ = new Vector3(0, targetYaw, 0);
            }
            else
            {
                AnimationController.animationMovementState = AnimationState.Idle;
                Entity.Get<CharacterComponent>().SetVelocity(Vector3.Zero);
                isActive = false;
            }
        }
        public bool IsValidManeuverGridPosition(GridPosition gridPosition)
        {
            List<GridPosition> validGridPositionList = GetValidManeuverGridPositionList();
            return validGridPositionList.Contains(gridPosition);
        }
        public List<GridPosition> GetValidManeuverGridPositionList()
        {
            List<GridPosition> validGridPositionList = new();

            GridPosition actorGridPosition = Actor.Get<Actor>().GridPosition;
            DebugText.Print($"{actorGridPosition}", new Int2(500, 100));
            for (int x = -MaxMoveDistance; x <= MaxMoveDistance; x++)
            {
                for (int y = -MaxMoveDistance; y <= MaxMoveDistance; y++)
                {
                    for (int z = -MaxMoveDistance; z <= MaxMoveDistance; z++)
                    {
                        GridPosition offsetGridPosition = new GridPosition(x, y, z);
                        GridPosition testGridPosition = actorGridPosition + offsetGridPosition;

                        if (!LevelGrid.GridSystem.IsValidGridPosition(testGridPosition)) continue;
                        if (actorGridPosition == testGridPosition) continue;
                        if (LevelGrid.HasAnyActorOnGridPosition(testGridPosition)) continue;

                        validGridPositionList.Add(testGridPosition);
                    }
                }
            }
            return validGridPositionList;
        }
        public void Move(GridPosition gridPosition)
        {
            isActive = true;
            targetPosition = LevelGrid.GridSystem.GetWorldPosition(gridPosition);
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
