using Star_Reverie.Globals;
using Star_Reverie.Maneuvers;
using Star_Reverie.Utils;
using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Input;
using System;
using Stride.UI;
using Stride.UI.Controls;
using Stride.UI.Events;
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
        private Grid maneuverGrid;
        private Entity maneuverUIEntity;
        private TextBlock attackStatusTextBlock;
        private TextBlock defendStatusTextBlock;
        private TextBlock turnNumber;
        public override void Start()
        {
            maneuverUIEntity = new();
            uIComponent = new UIComponent { Page = ManeuverUI };
            maneuverUIEntity.Add(uIComponent);
            maneuverList = ManeuverUI.RootElement.FindVisualChildOfType<StackPanel>("ManeuverList");
            maneuverGrid = ManeuverUI.RootElement.FindName("CombatUI") as Grid;
            attackStatusTextBlock = ManeuverUI.RootElement.FindVisualChildOfType<TextBlock>("AttackStatus");
            defendStatusTextBlock = ManeuverUI.RootElement.FindVisualChildOfType<TextBlock>("DefendStatus");
            turnNumber = ManeuverUI.RootElement.FindVisualChildOfType<TextBlock>("TurnNumber");

            ManeuverUI.RootElement.FindVisualChildOfType<Button>("EndTurnButton").Click
                += (object sender, RoutedEventArgs e) => NextTurn();
            ActorActionSystem.OnSelectedActorChanged += ActorActionSystem_OnSelectedActorChanged;
            ActorActionSystem.OnManeuverStarted += ActorActionSystem_OnManeuverStarted;
            CreateActorManeuverButtons();
            ActorManeuverStatus();
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
                ActorActionSystem.IsOverUI = UIHelper.IsPointerOverUI(uIComponent);
                if (!OnScreen)
                {
                    maneuverGrid.Visibility = Visibility.Visible;
   
                    OnScreen = true;
                }

            }
        }

        private void ActorManeuverStatus()
        {
            Actor actor = ActorActionSystem.Actor.Get<Actor>();

            if (attackStatusTextBlock != null)
            {
                attackStatusTextBlock.Text = actor.DidOffensiveManeuver ? "Can't Attack" : "Can Attack";
            }

            if (defendStatusTextBlock != null)
            {
                defendStatusTextBlock.Text = actor.DidDefensiveManeuver ? "Can't Defend" : "Can Defend";
            }
        }
        private void ActorActionSystem_OnSelectedActorChanged(object sender, EventArgs e)
        {
            CreateActorManeuverButtons();
            ActorManeuverStatus();

        }
        private void ActorActionSystem_OnManeuverStarted(object sender, EventArgs e)
        {
            ActorManeuverStatus();
        }
        private void CreateActorManeuverButtons()
        {
            maneuverList.Children.Clear();
            Actor selectedActor = ActorActionSystem.Actor.Get<Actor>();
            foreach (BaseManeuver baseManeuver in selectedActor.BaseManeuvers)
            {
                Button currentManeuver = (Button)ManeuverButton
                    .Instantiate()
                    .First()
                    .Get<UIComponent>().Page
                    .RootElement;

                currentManeuver.FindVisualChildOfType<TextBlock>().Text = baseManeuver.Name;
                currentManeuver.Click += (object sender, RoutedEventArgs e) =>
                        ActorActionSystem.SelectedManeuver = baseManeuver;

                maneuverList.Children.Add(currentManeuver);
            }
        }

        public void NextTurn()
        {
            turnNumber.Text = (Convert.ToInt32(turnNumber.Text) + 1).ToString();
        }
    }
}
