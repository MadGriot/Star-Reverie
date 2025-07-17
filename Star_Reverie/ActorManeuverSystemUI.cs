using Star_Reverie.Globals;
using Star_Reverie.Maneuvers;
using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Input;
using Stride.UI;
using Stride.UI.Controls;
using Stride.UI.Panels;
using System.Linq;

namespace Star_Reverie
{
    public class ActorManeuverSystemUI : SyncScript
    {
        public UIPage ManeuverUI;
        public Prefab ManeuverButton;
        private UIComponent uIComponent;
        private bool OnScreen = false;
        public ActorActionSystem ActorActionSystem;
        private StackPanel maneuverList;
        private Entity maneuverUIEntity;
        public override void Start()
        {
            maneuverUIEntity = new();
            uIComponent = new UIComponent { Page = ManeuverUI };
            maneuverUIEntity.Add(uIComponent);
            CreateActorManeuverButtons();
            SceneSystem.SceneInstance.RootScene.Entities.Add(maneuverUIEntity);

        }

        public override void Update()
        {
            if (maneuverList != null)
            {
                DebugText.Print($"ManeuverList is present {maneuverList.Visibility.ToString()}", new Int2(100, 100));
            }
            else
            {
                DebugText.Print("ManeuverList not found", new Int2(100, 100));
            }
            if (CurrentGameState.GameState == GameState.Encounter)
            {
                if (!OnScreen)
                {
                    maneuverList.Visibility = Visibility.Visible;
   
                    OnScreen = true;
                }

            }
        }

        private void CreateActorManeuverButtons()
        {
            Actor selectedActor = ActorActionSystem.Actor.Get<Actor>();
            maneuverList = ManeuverUI.RootElement.FindVisualChildOfType<StackPanel>("ManeuverList");
            foreach (BaseManeuver baseManeuver in selectedActor.BaseManeuvers)
            {
                Button currentManeuver = (Button)ManeuverButton
                    .Instantiate()
                    .First()
                    .Get<UIComponent>().Page
                    .RootElement;
                currentManeuver.FindVisualChildOfType<TextBlock>().Text = baseManeuver.Name;
                maneuverList.Children.Add(currentManeuver);
            }
        }
    }
}
