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
        internal Grid ItemSelection { get; set; }
        internal Grid Weapons { get; set; }
        internal Grid CreateWeapon { get; set; }
        internal Grid Armor { get; set; }
        internal Grid CreateArmor { get; set; }
        internal Grid Shields { get; set; }
        internal Grid CreateShield { get; set; }

        public override void Start()
        {
            ItemSelection = ItemCreationLibrary.InstantiateElement<Grid>("ItemSelection");
            Weapons = ItemCreationLibrary.InstantiateElement<Grid>("Weapons");
            CreateWeapon = ItemCreationLibrary.InstantiateElement<Grid>("CreateWeapon");
            Armor = ItemCreationLibrary.InstantiateElement<Grid>("Armor");
            CreateArmor = ItemCreationLibrary.InstantiateElement<Grid>("CreateArmor");
            Shields = ItemCreationLibrary.InstantiateElement<Grid>("Shields");
            CreateShield = ItemCreationLibrary.InstantiateElement<Grid>("CreateShield");

            BindNavigationButton("WeaponListButton", Weapons);
            BindNavigationButton("ArmorListButton", Armor);
            BindNavigationButton("ShieldListButton", Shields);

            BindButtons(Weapons, CreateWeapon);
            BindButtons(Armor, CreateArmor);
            BindButtons(Shields, CreateShield);

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
                    ItemUIPage.RootElement = ItemSelection;
                    CurrentGameState.GameState = GameState.Dialogue;
                }

            }
        }

        private void BindNavigationButton(string buttonName, Grid targetUI)
        {
            Button button = ItemSelection.FindVisualChildOfType<Button>(buttonName);
            button.Click += (object sender, RoutedEventArgs e) => ChangeUI(targetUI);
        }

        private void BindButtons(Grid gridUI, Grid creationUI)
        {
            Button backButton = gridUI.FindVisualChildOfType<Button>("BackButton");
            Button createButton = gridUI.FindVisualChildOfType<Button>("Create");
            backButton.Click += (object sender, RoutedEventArgs e) => GoBackToSelectionList();
            createButton.Click += (object sender, RoutedEventArgs e) => ChangeUI(creationUI);
        }
        private void ChangeUI(Grid UI) =>
                ItemUIPage.RootElement = UI;

        private void GoBackToSelectionList() =>
            ItemUIPage.RootElement = ItemSelection;
    }
}
