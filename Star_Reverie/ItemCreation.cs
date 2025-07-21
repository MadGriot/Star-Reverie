using Stride.Input;
using Stride.Engine;
using Stride.UI.Panels;
using Stride.UI;
using Star_Reverie.Globals;
using Stride.UI.Controls;
using Stride.UI.Events;

namespace Star_Reverie
{
    public class ItemCreation : SyncScript
    {
        public UILibrary ItemCreationLibrary { get; set; }
        public UIPage ItemUIPage { get; set; }
        private Grid itemSelection { get; set; }
        private Grid weapons { get; set; }
        private Grid armor { get; set; }
        private Grid shields { get; set; }

        public override void Start()
        {
            itemSelection = ItemCreationLibrary.InstantiateElement<Grid>("ItemSelection");
            weapons = ItemCreationLibrary.InstantiateElement<Grid>("Weapons");
            armor = ItemCreationLibrary.InstantiateElement<Grid>("Armor");
            shields = ItemCreationLibrary.InstantiateElement<Grid>("Shields");

            BindNavigationButton("WeaponListButton", weapons);
            BindNavigationButton("ArmorListButton", armor);
            BindNavigationButton("ShieldListButton", shields);

            BindBackButton(weapons);
            BindBackButton(armor);
            BindBackButton(shields);

            //Adding UI page
            Entity uiEntity = [new UIComponent { Page = ItemUIPage }];
            Entity.Scene.Entities.Add(uiEntity);
        }

        public override void Update()
        {
            if (Input.IsKeyPressed(Keys.I))
            {
                if (CurrentGameState.GameState != GameState.Dialogue)
                {
                    ItemUIPage.RootElement = itemSelection;
                    CurrentGameState.GameState = GameState.Dialogue;
                }

            }
        }

        private void BindNavigationButton(string buttonName, Grid targetUI)
        {
            Button button = itemSelection.FindVisualChildOfType<Button>(buttonName);
            button.Click += (object sender, RoutedEventArgs e) => ChangeUI(targetUI);
        }

        private void BindBackButton(Grid gridUI)
        {
            Button backButton = gridUI.FindVisualChildOfType<Button>("BackButton");
            backButton.Click += (object sender, RoutedEventArgs e) => GoBackToSelectionList();
        }
        private void ChangeUI(Grid UI) =>
                ItemUIPage.RootElement = UI;

        private void GoBackToSelectionList() =>
            ItemUIPage.RootElement = itemSelection;
    }
}
