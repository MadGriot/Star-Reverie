using Stride.Input;
using Stride.Engine;
using Stride.UI.Panels;
using Stride.UI;
using Star_Reverie.Globals;

namespace Star_Reverie
{
    public class ItemCreation : SyncScript
    {
        public UILibrary ItemCreationLibrary { get; set; }
        public UIPage ItemUIPage { get; set; }
        private Grid itemSelection { get; set; }
        private Grid weapons { get; set; }

        public override void Start()
        {
            itemSelection = ItemCreationLibrary.InstantiateElement<Grid>("ItemSelection");
            weapons = ItemCreationLibrary.InstantiateElement<Grid>("Weapons");

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
    }
}
