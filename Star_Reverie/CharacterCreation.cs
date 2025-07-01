using Stride.Input;
using Stride.Engine;
using Stride.UI.Controls;
using Stride.UI;
using Stride.UI.Events;

namespace Star_Reverie
{
    public class CharacterCreation : SyncScript
    {
        public UIPage CharacterCreationMain;
        private Entity uiEntity;
        private UIComponent uIComponent;
        private bool OnScreen = false;

        //TextBlocks
        TextBlock characterPoints;
        TextBlock ageNumber;
        public override void Start()
        {
            uiEntity = new();
            uIComponent = new();
            uIComponent.Page = CharacterCreationMain;
            uiEntity.Add(uIComponent);
            StatNumberMapping();
            ButtonEventMapping();
        }

        public override void Update()
        {
            if (Input.IsKeyPressed(Keys.C))
            {
                if (!OnScreen)
                {
                    SceneSystem.SceneInstance.RootScene.Entities.Add(uiEntity);
                    OnScreen = true;
                }

            }
        }
        private void StatNumberMapping()
        {
            characterPoints = CharacterCreationMain.RootElement.FindVisualChildOfType<TextBlock>("CharacterPoints");
            ageNumber = CharacterCreationMain.RootElement.FindVisualChildOfType<TextBlock>("AgeNumber");
        }
        private void ButtonEventMapping()
        {
            Button addAge = CharacterCreationMain.RootElement.FindVisualChildOfType<Button>("AddAge");
            Button subtractAge = CharacterCreationMain.RootElement.FindVisualChildOfType<Button>("SubtractAge");

            addAge.Click += (object sender, RoutedEventArgs e) => ChangeAge(true);
        }

        private void ChangeAge(bool add)
        {
            int currentAge = int.Parse(ageNumber.Text);
            if (currentAge < 45 && add)
            {
                currentAge += 1;

            }
            if (currentAge > 21 && !add)
            {
                currentAge -= 1;
            }
            ageNumber.Text = currentAge.ToString();
        }
    }
}
