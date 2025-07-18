using StarReverieCore.Grid;
using Stride.Core.Mathematics;
using Stride.Physics;
using System;
using System.Collections.Generic;

namespace Star_Reverie.Maneuvers
{
    public class SpinManeuver : BaseManeuver
    {
        // Declared public member fields and properties will show in the game studio
        private float totalSpinAmount;

        public override void Start()
        {
            Name = "Spin";
        }

        public override void Update()
        {
            if (!isActive) return;

            float spinAddAmount = 360f * (float)Game.UpdateTime.Elapsed.TotalSeconds;
            Quaternion currentRotation = Actor.Transform.Rotation;
            Quaternion rotationDelta = Quaternion.RotationY(spinAddAmount);

            Actor.Get<CharacterComponent>().Orientation = currentRotation * rotationDelta;
            currentRotation = currentRotation * rotationDelta;
            totalSpinAmount += spinAddAmount;
            if (totalSpinAmount >= 360f)
            {
                isActive = false;
                OnActionComplete();

            }
        }

        public override void ActivateManeuver(GridPosition gridPosition, Action onActionComplete)
        {
            OnActionComplete = onActionComplete;
            isActive = true;
            totalSpinAmount = 0f;
        }

        public override List<GridPosition> GetValidManeuverGridPositionList()
        {
            GridPosition actorGridPosition = Actor.Get<Actor>().GridPosition;

            return new List<GridPosition> { actorGridPosition };
        }
    }
}
