using Star_Reverie.Globals;
using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Graphics;
using Stride.Input;
using Stride.Physics;

namespace Star_Reverie
{
    public class MouseWorld : SyncScript
    {
        public CameraComponent camera;
        private Simulation simulation;
        private Vector3 targetPosition;
        private Vector3 currentPosition;
        private CharacterComponent characterComponent;
        public override void Start()
        {
            characterComponent = Entity.Get<CharacterComponent>();
            simulation = this.GetSimulation();
        }

        public override void Update()
        {
            if (CurrentGameState.GameState != GameState.Encounter)
                return;

            float stoppingDistance = 0.1f;
            currentPosition = Entity.Transform.Position;
            if (Vector3.Distance(currentPosition, targetPosition) > stoppingDistance)
            {
                Vector3 velocity = Vector3.Normalize(targetPosition - currentPosition);
                float moveSpeed = 4f;
                float deltaTime = (float)Game.UpdateTime.Elapsed.TotalSeconds;
                characterComponent.SetVelocity(velocity * moveSpeed);
            }
            else
            {
                characterComponent.SetVelocity(Vector3.Zero);
            }
            if (Input.IsMouseButtonPressed(MouseButton.Left))
            {
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

        private void Move(Vector3 targetPosition)
        {
            this.targetPosition = targetPosition;
        }
    }
}
