using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Graphics;
using Stride.Input;
using Stride.Physics;

namespace Star_Reverie
{
    public class ActorActionSystem : SyncScript
    {
        // Declared public member fields and properties will show in the game studio
        public Entity Actor { get; set; }
        internal MouseWorld MouseWorld { get; set; }

        public override void Start()
        {
            MouseWorld = Actor.Get<MouseWorld>();
        }

        public override void Update()
        {
            if (Input.IsMouseButtonPressed(MouseButton.Left))
            {
                HandleActorSelection();
                MouseWorld.Move(MouseWorld.GetPosition());
                MouseWorld.animationState = AnimationState.Running;
                MouseWorld.PlayAnimation(MouseWorld.animationState);


            }

            DebugText.Print($"{Actor.Name}", new Int2(700, 600));
        }

        public void HandleActorSelection()
        {
            Texture backbuffer = GraphicsDevice.Presenter.BackBuffer;
            Viewport viewport = new Viewport(0, 0, backbuffer.Width, backbuffer.Height);

            Vector3 nearPosition = viewport.Unproject(new Vector3(Input.AbsoluteMousePosition, 0),
                MouseWorld.camera.ProjectionMatrix, MouseWorld.camera.ViewMatrix, Matrix.Identity);

            Vector3 farPosition = viewport.Unproject(new Vector3(Input.AbsoluteMousePosition, 1.0f),
                MouseWorld.camera.ProjectionMatrix, MouseWorld.camera.ViewMatrix, Matrix.Identity);

            MouseWorld.simulation.Raycast(nearPosition, farPosition,
                out HitResult hitResult, CollisionFilterGroups.CustomFilter1, MouseWorld.CollideWith, MouseWorld.CollideWithTriggers);

            if (hitResult.Succeeded)
            {
                MouseWorld.actorSelected = false;
                Actor = hitResult.Collider.Entity;
                MouseWorld = Actor.Get<MouseWorld>();
                MouseWorld.actorSelected = true;
            }
        }
    }
}
