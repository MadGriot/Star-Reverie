using Star_Reverie.Globals;
using Stride.Engine;
using Stride.Input;

namespace Star_Reverie
{
    public class BattleUI : SyncScript
    {
        public UIPage ManeuverUI;
        private bool OnScreen = false;

        private Entity maneuverUIEntity;
        public override void Start()
        {
            maneuverUIEntity = new();
            UIComponent uIComponent = new UIComponent { Page = ManeuverUI };
            maneuverUIEntity.Add(uIComponent);
        }

        public override void Update()
        {
            if (Input.IsKeyPressed(Keys.Q))
            {
                if (!OnScreen && CurrentGameState.GameState != GameState.Encounter)
                {
                    SceneSystem.SceneInstance.RootScene.Entities.Add(maneuverUIEntity);
                    OnScreen = true;
                }

            }
        }
    }
}
