using Star_Reverie.Globals;
using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Input;
using Stride.Physics;

namespace Star_Reverie
{
    public class Actor : SyncScript
    {

        public LevelGrid LevelGrid;
        public override void Start()
        {
            LevelGrid.AddActorAtGridPosition
                (LevelGrid.GridSystem.GetGridPosition(Entity.Transform.Position), this);
        }

        public override void Update()
        {

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
