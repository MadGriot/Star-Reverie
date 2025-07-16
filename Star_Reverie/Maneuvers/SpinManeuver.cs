using System;
using Stride.Core.Mathematics;
using Stride.Physics;

namespace Star_Reverie.Maneuvers
{
    public class SpinManeuver : BaseManeuver
    {
        // Declared public member fields and properties will show in the game studio
        private float totalSpinAmount;

        public override void Start()
        {
            // Initialization of the script.
        }

        public override void Update()
        {
            if (!isActive) return;

            float spinAddAmount = 360f * (float)Game.UpdateTime.Elapsed.TotalSeconds;
            Quaternion currentRotation = Actor.Transform.Rotation;
            Quaternion rotationDelta = Quaternion.RotationY(spinAddAmount);

            Actor.Get<CharacterComponent>().Orientation = currentRotation * rotationDelta;
            totalSpinAmount += spinAddAmount;
            if (totalSpinAmount >= 360f)
            {
                isActive = false;
                OnActionComplete();

            }
        }

        public void Spin(Action onActionComplete)
        {
            OnActionComplete = onActionComplete;
            isActive = true;
            totalSpinAmount = 0f;
        }
    }
}
