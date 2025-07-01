using Stride.Input;
using Stride.Engine;
using Stride.UI.Controls;
using Stride.UI;
using Stride.UI.Events;
using System;

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
        TextBlock strengthNumber;
        TextBlock hitPointsNumber;
        TextBlock basicLiftNumber;
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
            characterPoints = CharacterCreationMain.RootElement.FindVisualChildOfType<TextBlock>("CharacterPointsNumber");
            ageNumber = CharacterCreationMain.RootElement.FindVisualChildOfType<TextBlock>("AgeNumber");
            strengthNumber = CharacterCreationMain.RootElement.FindVisualChildOfType<TextBlock>("StrengthNumber");
            hitPointsNumber = CharacterCreationMain.RootElement.FindVisualChildOfType<TextBlock>("HitPointsNumber");
            basicLiftNumber = CharacterCreationMain.RootElement.FindVisualChildOfType<TextBlock>("BasicLiftNumber");
        }
        private void ButtonEventMapping()
        {
            Button addAge = CharacterCreationMain.RootElement.FindVisualChildOfType<Button>("AddAge");
            Button subtractAge = CharacterCreationMain.RootElement.FindVisualChildOfType<Button>("SubtractAge");
            Button addStrength = CharacterCreationMain.RootElement.FindVisualChildOfType<Button>("AddStrength");
            Button subtractStrength = CharacterCreationMain.RootElement.FindVisualChildOfType<Button>("SubtractStrength");

            addAge.Click += (object sender, RoutedEventArgs e) => ChangeAge(true);
            subtractAge.Click += (object sender, RoutedEventArgs e) => ChangeAge(false);
            addStrength.Click += (object sender, RoutedEventArgs e) => ChangeAttribute("Strength", true);
            subtractStrength.Click += (object sender, RoutedEventArgs e) => ChangeAttribute("Strength", false);
            
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

        private void ChangeAttribute(string attribute, bool add)
        {
            int currentCharacterPoints = int.Parse(characterPoints.Text);
            int currentStrength = int.Parse(strengthNumber.Text);
            int currentHitPoints = int.Parse(hitPointsNumber.Text);
            int currentBasicLift = int.Parse(basicLiftNumber.Text);
            switch (attribute)
            {
                case "Strength":
                    if (currentCharacterPoints >= 10 && add)
                    {
                        currentStrength += 1;
                        currentHitPoints += 1;
                        currentBasicLift = (currentStrength * currentStrength) / 5;
                        currentCharacterPoints -= 10;
                        

                    }
                    else if (currentCharacterPoints >= 10 && !add && currentStrength > 3)
                    {
                        currentStrength -= 1;
                        currentHitPoints -= 1;
                        currentBasicLift = (currentStrength * currentStrength) / 5;
                        currentCharacterPoints += 10;
                    }
                    strengthNumber.Text = currentStrength.ToString();
                    characterPoints.Text = currentCharacterPoints.ToString();
                    hitPointsNumber.Text = currentHitPoints.ToString();
                    basicLiftNumber.Text = currentBasicLift.ToString();
                    break;

                case "Dexterity":
                    if (currentCharacterPoints >= 20)
                    {

                    }
                    break;
            }
        }
    }
}
