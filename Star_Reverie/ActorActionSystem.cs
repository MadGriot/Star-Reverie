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
        internal bool isBusy { get; set; }
        public override void Start()
        {

        }
        public override void Update()
        {
            if (CurrentGameState.GameState != GameState.Encounter)
                return;
            if (isBusy) return;
            if (Input.IsMouseButtonPressed(MouseButton.Left))
            {
                bool selectionChanged = TryHandleActorSelection();
                if (Actor.Get<Actor>().actorSelected)
                {
                    GridPosition mouseGridPosition = LevelGrid.GridSystem.GetGridPosition(MouseWorld.GetPosition());
                    if (!selectionChanged && Actor.Get<MoveManeuver>().IsValidManeuverGridPosition(mouseGridPosition))
                    {
                        SetBusy();
                        Actor.Get<MoveManeuver>().Move(mouseGridPosition, ClearBusy);
                    }
                }
            }

            if (Input.IsMouseButtonPressed(MouseButton.Middle))
            {
                if (Actor.Get<Actor>().actorSelected)
                {
                    SetBusy();
                    Actor.Get<SpinManeuver>().Spin(ClearBusy);
                }
            }

            DebugText.Print($"{Actor.Name}", new Int2(700, 600));

        }
        private void SetBusy() => isBusy = true;
        private void ClearBusy() => isBusy = false;

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
                Entity clickedEntity = hitResult.Collider.Entity;
                if (clickedEntity != Actor)
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
            }
            return false;
        }
    }
}
