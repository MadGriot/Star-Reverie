using Star_Reverie.Globals;
using Star_Reverie.Maneuvers;
using StarReverieCore.Grid;
using StarReverieCore.Models;
using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Input;
using Stride.Physics;
using System.Linq;

namespace Star_Reverie
{
    public class Actor : SyncScript
    {
        internal GridPosition GridPosition;
        public LevelGrid LevelGrid;
        public bool actorSelected;
        public int CharacterId;
        internal Character Character;
        private AnimationController AnimationController;
        internal BaseManeuver[] BaseManeuvers;
        public override void Start()
        {
            GridPosition = LevelGrid.GridSystem.GetGridPosition(Entity.Transform.Position);
            AnimationController = Entity.Get<AnimationController>();
            AnimationController.animationComponent = Entity.GetChild(1).Get<AnimationComponent>();
            AnimationController.animationComponent.Play("Idle");
            LevelGrid.AddActorAtGridPosition
                (LevelGrid.GridSystem.GetGridPosition(Entity.Transform.Position), this);

            BaseManeuvers = Entity.GetAll<BaseManeuver>().ToArray();
            Character = Database.StarReverieDbContext.Characters.Find(CharacterId);
            Entity.Name = Character.FirstName;
        }

        public override void Update()
        {
            if (actorSelected)
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
                if (CurrentGameState.GameState != GameState.Encounter)
                {
                    if (Input.IsKeyPressed(Keys.Q))
                    {
                        CurrentGameState.GameState = GameState.Encounter;
                    }

                    return;
                }
            }

        }


    }
}
