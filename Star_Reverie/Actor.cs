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
        public bool actorSelected;
        private AnimationController AnimationController;
        public override void Start()
        {
            AnimationController = Entity.Get<AnimationController>();
            AnimationController.animationComponent = Entity.GetChild(1).Get<AnimationComponent>();
            AnimationController.animationComponent.Play("Idle");
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
