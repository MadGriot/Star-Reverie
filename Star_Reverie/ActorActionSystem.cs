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
        internal bool IsOverUI;
        public event EventHandler OnSelectedActorChanged;

        internal BaseManeuver SelectedManeuver;
        internal bool isBusy { get; set; }

        public override void Start()
        {
            SetSelectedActor(Actor);
        }
        public override void Update()
        {
            if (CurrentGameState.GameState != GameState.Encounter) return;
            if (isBusy) return;
            if (IsOverUI) return;

            if (TryHandleActorSelection()) return;

            HandleSelectedManeuver();

            DebugText.Print($"{Actor.Name}", new Int2(700, 600));

        }

        private void HandleSelectedManeuver()
        {
            if (Input.IsMouseButtonDown(MouseButton.Left))
            {
                GridPosition mouseGridPosition = LevelGrid.GridSystem.GetGridPosition(MouseWorld.GetPosition());

                if (SelectedManeuver.IsValidManeuverGridPosition(mouseGridPosition))
                {
                    SetBusy();
                    SelectedManeuver.ActivateManeuver(mouseGridPosition, ClearBusy);

                }
            }
        }
        private void SetBusy() => isBusy = true;
        private void ClearBusy() => isBusy = false;

        public bool TryHandleActorSelection()
        {
            if (Input.IsMouseButtonPressed(MouseButton.Left))
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

                        SetSelectedActor(clickedEntity);
                        return true;
                    }
                }
            }
            return false;
        }

        private void SetSelectedActor(Entity actor)
        {
            Actor.Get<Actor>().actorSelected = false;
            CameraComponent mainCamera = Actor.GetChild(0).GetChild(0).Get<CameraComponent>();
            Actor = actor;
            SelectedManeuver = actor.Get<MoveManeuver>();
            //Camera Swapping
            CameraComponent targetCamera = Actor.GetChild(0).GetChild(0).Get<CameraComponent>();
            targetCamera.Enabled = !targetCamera.Enabled;
            MouseWorld.Camera = targetCamera;
            mainCamera.Enabled = !mainCamera.Enabled;


            Actor.Get<Actor>().actorSelected = true;
            OnSelectedActorChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
