using Star_Reverie.Globals;
using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Input;
using Stride.Physics;

namespace Star_Reverie
{
    public class Actor : SyncScript
    {


        public override void Start()
        {

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
