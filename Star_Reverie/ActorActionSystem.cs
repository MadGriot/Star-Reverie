using Star_Reverie.Globals;
using Star_Reverie.Maneuvers;
using StarReverieCore.Grid;
using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Graphics;
using Stride.Input;
using Stride.Physics;
using System;
using System.Collections.Generic;
using System.Linq;

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
        public event EventHandler OnManeuverStarted;
        public event EventHandler OnEncounterStarted;
        internal List<Entity> TurnQueue { get; set; } = new();

        internal BaseManeuver SelectedManeuver;
        internal bool isBusy { get; set; }
        internal bool isPlayerTurn { get; set; } = true;
        public override void Start()
        {
            SetSelectedActor(Actor);
        }
        public override void Update()
        {

            if (CurrentGameState.GameState != GameState.Encounter)
            {
                if (Input.IsKeyPressed(Keys.Q))
                {
                    CurrentGameState.GameState = GameState.Encounter;
                    OnEncounterStarted?.Invoke(this, EventArgs.Empty);
                }
                return;
            }

            if (isBusy) return;
            if (IsOverUI) return;

            //if (TryHandleActorSelection()) return;
            if (Actor.Get<Actor>().IsEnemy)
            {
                return;
            }
            HandleSelectedManeuver();

            DebugText.Print($"{Actor.Name}", new Int2(700, 600));

            for (int i = 0; i < TurnQueue.Count; i++)
            {
                DebugText.Print($"Characters in Combat: {TurnQueue.ToArray()[i].Name} Speed: {TurnQueue.ToArray()[i].Get<Actor>().Character.AttributeScore.Speed}", new Int2(700, 700 + (i * 50)));

            }
        }

        private void HandleSelectedManeuver()
        {
            if (Input.IsMouseButtonDown(MouseButton.Left))
            {
                GridPosition mouseGridPosition = LevelGrid.GridSystem.GetGridPosition(MouseWorld.GetPosition());

                if (SelectedManeuver.IsValidManeuverGridPosition(mouseGridPosition))
                {
                    if (Actor.Get<Actor>().TryToDoManeuver(SelectedManeuver))
                    {
                        SetBusy();
                        SelectedManeuver.ActivateManeuver(mouseGridPosition, ClearBusy);

                        OnManeuverStarted?.Invoke(this, EventArgs.Empty);
                    }

                }
            }
        }
        private void SetBusy() => isBusy = true;
        private void ClearBusy() => isBusy = false;

        //public bool TryHandleActorSelection()
        //{
        //    if (Input.IsMouseButtonPressed(MouseButton.Left))
        //    {
        //        Texture backbuffer = GraphicsDevice.Presenter.BackBuffer;
        //        Viewport viewport = new Viewport(0, 0, backbuffer.Width, backbuffer.Height);

        //        Vector3 nearPosition = viewport.Unproject(new Vector3(Input.AbsoluteMousePosition, 0),
        //            MouseWorld.Camera.ProjectionMatrix, MouseWorld.Camera.ViewMatrix, Matrix.Identity);

        //        Vector3 farPosition = viewport.Unproject(new Vector3(Input.AbsoluteMousePosition, 1.0f),
        //            MouseWorld.Camera.ProjectionMatrix, MouseWorld.Camera.ViewMatrix, Matrix.Identity);

        //        MouseWorld.simulation.Raycast(nearPosition, farPosition,
        //            out HitResult hitResult, CollisionFilterGroups.CustomFilter1, MouseWorld.CollideWith, MouseWorld.CollideWithTriggers);


        //        if (hitResult.Succeeded)
        //        {
        //            Entity clickedEntity = hitResult.Collider.Entity;
        //            if (clickedEntity != Actor)
        //            {
        //                if (clickedEntity.Get<Actor>().IsEnemy)
        //                    return false;
        //                SetSelectedActor(clickedEntity);
        //                return true;
        //            }
        //        }
        //    }
        //    return false;
        //}

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

        public void TurnEnded()
        {
            //if (TurnQueue.First().Equals(Actor)) return;
            Entity actor = TurnQueue[0];
            TurnQueue.RemoveAt(0);
            actor.Get<Actor>().DidDefensiveManeuver = false;
            actor.Get<Actor>().DidOffensiveManeuver = false;
            TurnQueue.Add(actor);
            isPlayerTurn = !actor.Get<Actor>().IsEnemy;
            SetSelectedActor(actor);
        }
    }
}
