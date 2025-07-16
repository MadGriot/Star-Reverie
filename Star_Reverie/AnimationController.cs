using Stride.Engine;

namespace Star_Reverie
{
    public class AnimationController : SyncScript
    {
        public float AnimationSpeed = 1.0f;
        internal AnimationComponent animationComponent;
        internal AnimationState animationState;
        internal AnimationState animationMovementState;

        public override void Start()
        {
            animationState = AnimationState.Idle;
        }

        public override void Update()
        {
            if (animationState != animationMovementState)
            {
                animationState = animationMovementState;
                PlayAnimation(animationState);
            }
        }
        public void StopMovement()
        {
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
