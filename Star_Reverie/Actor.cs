using Microsoft.EntityFrameworkCore;
using Star_Reverie.Globals;
using Star_Reverie.Maneuvers;
using StarReverieCore.Grid;
using StarReverieCore.Models;
using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Input;
using System.Collections.Generic;
using System.Linq;

namespace Star_Reverie
{
    public class Actor : SyncScript
    {
        internal GridPosition GridPosition;
        internal LevelGrid LevelGrid;
        internal ActorActionSystem ActorActionSystem;
        public bool actorSelected;
        public int CharacterId;
        public bool IsEnemy;
        internal Character Character;
        private AnimationController AnimationController;
        internal BaseManeuver[] BaseManeuvers;
        internal bool InCombat;

        internal bool DidOffensiveManeuver;
        internal bool DidDefensiveManeuver;
        public override void Start()
        {
            LevelGrid = SceneSystem.SceneInstance.RootScene.Entities
                .First(e => e.Name == "LevelGrid")
                .Get<LevelGrid>();
            ActorActionSystem = SceneSystem.SceneInstance.RootScene.Entities
                .First(e => e.Name == "ActorActionSystem")
                .Get<ActorActionSystem>();

            ActorActionSystem.OnEncounterStarted += ActorActionSystem_OnEncounterStarted;
            GridPosition = LevelGrid.GridSystem.GetGridPosition(Entity.Transform.Position);
            AnimationController = Entity.Get<AnimationController>();
            AnimationController.animationComponent = Entity.GetChild(1).Get<AnimationComponent>();
            AnimationController.animationComponent.Play("Idle");
            LevelGrid.AddActorAtGridPosition
                (LevelGrid.GridSystem.GetGridPosition(Entity.Transform.Position), this);
            
            BaseManeuvers = Entity.GetAll<BaseManeuver>().ToArray();
            Character = Database.StarReverieDbContext.Characters
                .Include(a => a.AttributeScore)
                .First(c => c.Id == CharacterId);
            Entity.Name = Character.FirstName;
        }



        public override void Update()
        {
            if (actorSelected && !IsEnemy)
            {
                DebugText.Print($" Current Position {Entity.Transform.Position}", new Int2(1000, 800));
                DebugText.Print($" Current Grid Position {GridPosition}", new Int2(1000, 900));
                DebugText.Print($" Maneuver Count {BaseManeuvers.Count()}", new Int2(1000, 950));
                GridPosition newGridPosition = LevelGrid.GridSystem.GetGridPosition(Entity.Transform.Position);
                if (newGridPosition != GridPosition)
                {
                    LevelGrid.ActorMovedGridPosition(this, GridPosition, newGridPosition);
                    GridPosition = newGridPosition;
                }
                if (CurrentGameState.GameState == GameState.Encounter)
                {
                    return;
                }

 

            }

        }
        private void ToggleCombatIfNotEnemy()
        {
            if (!IsEnemy)
                InCombat = !InCombat;
            ActorActionSystem.TurnQueue.Add(Entity);
        }

        private void ToggleCombatIfEnemy()
        {
            if (IsEnemy)
                InCombat = !InCombat;
            ActorActionSystem.TurnQueue.Add(Entity);
            ActorActionSystem.TurnQueue = ActorActionSystem.TurnQueue
                .OrderByDescending(a => a.Get<Actor>().Character.AttributeScore.Speed).ToList();
        }
        public bool TryToDoManeuver(BaseManeuver baseManeuver)
        {
            if(CanDoManeuver(baseManeuver))
            {
                if (baseManeuver.IsOffensive) DidOffensiveManeuver = true;
                if (!baseManeuver.IsOffensive) DidDefensiveManeuver = true;
                return true;
            }
            else return false;
        }
        public bool CanDoManeuver(BaseManeuver baseManeuver)
        {
            if (!DidOffensiveManeuver && baseManeuver.IsOffensive) return true;
            if (!DidDefensiveManeuver && !baseManeuver.IsOffensive) return true;
            else return false;
        }
        private void ActorActionSystem_OnEncounterStarted(object sender, System.EventArgs e)
        {
            ToggleCombatIfEnemy();
        }
    }
}
