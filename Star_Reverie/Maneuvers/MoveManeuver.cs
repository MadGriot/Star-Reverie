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
        private AnimationController AnimationController;
        private CharacterComponent characterComponent;
        public int MaxMoveDistance = 3;


        //Debugging
        private float debugTimer = 0f;
        private string debugMessage = "";
        public override void Start()
        {
            Name = "Move";
            AnimationController = Actor.Get<AnimationController>();
            characterComponent = Actor.Get<CharacterComponent>();
            targetPosition = Actor.Transform.Position;
        }

        public override void Update()
        {
            float debugDeltaTime = (float)Game.UpdateTime.Elapsed.TotalSeconds;
            if (debugTimer > 0f)
            {
                DebugText.Print(debugMessage, new Int2(200, 600), new Color4(Color.Beige));
                debugTimer -= debugDeltaTime;
            }
            if (CurrentGameState.GameState != GameState.Encounter)
                return;
            if (!Actor.Get<Actor>().actorSelected) return;
            else
                DebugText.Print($"Target Position: {targetPosition}", new Int2(700, 800));
            if (!isActive) return;


            float stoppingDistance = 0.1f;
            currentPosition = Actor.Transform.Position;
            if (Vector3.Distance(currentPosition, targetPosition) > stoppingDistance)
            {
                AnimationController.animationMovementState = AnimationState.Running;
                Vector3 velocity = Vector3.Normalize(targetPosition - currentPosition);
                float moveSpeed = 6f;
                float deltaTime = (float)Game.UpdateTime.Elapsed.TotalSeconds;
                characterComponent.SetVelocity(velocity * moveSpeed);

                float targetYaw = (float)Math.Atan2(velocity.X, velocity.Z); // returns radians

                Actor.GetChild(1).Transform.RotationEulerXYZ = new Vector3(0, targetYaw, 0);
            }
            else
            {
                AnimationController.animationMovementState = AnimationState.Idle;
                ResetTarget();
                Actor.Get<CharacterComponent>().SetVelocity(Vector3.Zero);
                isActive = false;
                OnActionComplete();
            }
        }

        public override List<GridPosition> GetValidManeuverGridPositionList()
        {
            List<GridPosition> validGridPositionList = new();

            GridPosition actorGridPosition = Actor.Get<Actor>().GridPosition;
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
        public override void ActivateManeuver(GridPosition gridPosition, Action onActionComplete)
        {
            debugMessage = $"Moving {Actor.Name} to {gridPosition}\nWorld Target Position: {LevelGrid.GridSystem.GetWorldPosition(gridPosition)}";
            debugTimer = 3f;

            OnActionComplete = onActionComplete;
            isActive = true;
            targetPosition = LevelGrid.GridSystem.GetWorldPosition(gridPosition);
            AnimationController.animationState = AnimationState.Running;
            AnimationController.PlayAnimation(AnimationState.Running);
        }

        public void ResetTarget()
        {
            AnimationController.StopMovement();
        }
    }
}
