using Star_Reverie.Globals;
using Star_Reverie.Maneuvers;
using StarReverieCore.Grid;
using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Graphics;
using Stride.Input;
using Stride.Physics;
using System;

namespace Star_Reverie
{
    public class ActorActionSystem : SyncScript
    {
        // Declared public member fields and properties will show in the game studio
        public Entity Actor { get; set; }
        public LevelGrid LevelGrid { get; set; }
        public MouseWorld MouseWorld { get; set; }
        public event EventHandler OnSelectedActorChanged;
        public override void Start()
        {

        }

        public override void Update()
        {
            if (CurrentGameState.GameState != GameState.Encounter)
                return;
            if (Input.IsMouseButtonPressed(MouseButton.Left))
            {
                TryHandleActorSelection();
                GridPosition mouseGridPosition = LevelGrid.GridSystem.GetGridPosition(MouseWorld.GetPosition());
                if (Actor.Get<Actor>().actorSelected && Actor.Get<MoveManeuver>().IsValidManeuverGridPosition(mouseGridPosition))
                {
                    Actor.Get<MoveManeuver>().Move(mouseGridPosition);
                }
            }

            DebugText.Print($"{Actor.Name}", new Int2(700, 600));

        }

        public bool TryHandleActorSelection()
        {
            Texture backbuffer = GraphicsDevice.Presenter.BackBuffer;
            Viewport viewport = new Viewport(0, 0, backbuffer.Width, backbuffer.Height);

            Vector3 nearPosition = viewport.Unproject(new Vector3(Input.AbsoluteMousePosition, 0),
                MouseWorld.Camera.ProjectionMatrix, MouseWorld.Camera.ViewMatrix, Matrix.Identity);

            Vector3 farPosition = viewport.Unproject(new Vector3(Input.AbsoluteMousePosition, 1.0f),
                MouseWorld.Camera.ProjectionMatrix, MouseWorld.Camera.ViewMatrix, Matrix.Identity);

            MouseWorld.simulation.Raycast(nearPosition, farPosition,
                out HitResult hitResult, CollisionFilterGroups.CustomFilter1, MouseWorld.CollideWith, MouseWorld.CollideWithTriggers);


            if (hitResult.Succeeded)
            {
                Actor.Get<Actor>().actorSelected = false;
                CameraComponent mainCamera = Actor.GetChild(0).GetChild(0).Get<CameraComponent>();
                Actor = hitResult.Collider.Entity;

                //Camera Swapping
                CameraComponent targetCamera = Actor.GetChild(0).GetChild(0).Get<CameraComponent>();
                targetCamera.Enabled = !targetCamera.Enabled;
                MouseWorld.Camera = targetCamera;
                mainCamera.Enabled = !mainCamera.Enabled;


                Actor.Get<Actor>().actorSelected = true;
                OnSelectedActorChanged?.Invoke(this, EventArgs.Empty);
                return true;
            }
            return false;
        }
    }
}
