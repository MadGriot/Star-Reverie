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
        TextBlock dexterityNumber;
        TextBlock intelligenceNumber;
        TextBlock constitutionNumber;
        TextBlock hitPointsNumber;
        TextBlock willNumber;
        TextBlock perceptionNumber;
        TextBlock staminaNumber;
        TextBlock basicLiftNumber;
        TextBlock speedNumber;
        TextBlock moveNumber;
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
            dexterityNumber = CharacterCreationMain.RootElement.FindVisualChildOfType<TextBlock>("DexterityNumber");
            intelligenceNumber = CharacterCreationMain.RootElement.FindVisualChildOfType<TextBlock>("IntelligenceNumber");
            constitutionNumber = CharacterCreationMain.RootElement.FindVisualChildOfType<TextBlock>("ConstitutionNumber");
            hitPointsNumber = CharacterCreationMain.RootElement.FindVisualChildOfType<TextBlock>("HitPointsNumber");
            willNumber = CharacterCreationMain.RootElement.FindVisualChildOfType<TextBlock>("WillNumber");
            perceptionNumber = CharacterCreationMain.RootElement.FindVisualChildOfType<TextBlock>("PerceptionNumber");
            staminaNumber = CharacterCreationMain.RootElement.FindVisualChildOfType<TextBlock>("StaminaNumber");
            basicLiftNumber = CharacterCreationMain.RootElement.FindVisualChildOfType<TextBlock>("BasicLiftNumber");
            speedNumber = CharacterCreationMain.RootElement.FindVisualChildOfType<TextBlock>("SpeedNumber");
            moveNumber = CharacterCreationMain.RootElement.FindVisualChildOfType<TextBlock>("MoveNumber");
        }
        private void ButtonEventMapping()
        {
            Button addAge = CharacterCreationMain.RootElement.FindVisualChildOfType<Button>("AddAge");
            Button subtractAge = CharacterCreationMain.RootElement.FindVisualChildOfType<Button>("SubtractAge");
            Button addStrength = CharacterCreationMain.RootElement.FindVisualChildOfType<Button>("AddStrength");
            Button subtractStrength = CharacterCreationMain.RootElement.FindVisualChildOfType<Button>("SubtractStrength");
            Button addDexterity = CharacterCreationMain.RootElement.FindVisualChildOfType<Button>("AddDexterity");
            Button subtractDexterity = CharacterCreationMain.RootElement.FindVisualChildOfType<Button>("SubtractDexterity");
            Button addIntelligence = CharacterCreationMain.RootElement.FindVisualChildOfType<Button>("AddIntelligence");
            Button subtractIntelligence = CharacterCreationMain.RootElement.FindVisualChildOfType<Button>("SubtractIntelligence");

            addAge.Click += (object sender, RoutedEventArgs e) => ChangeAge(true);
            subtractAge.Click += (object sender, RoutedEventArgs e) => ChangeAge(false);
            addStrength.Click += (object sender, RoutedEventArgs e) => ChangeAttribute("Strength", true);
            subtractStrength.Click += (object sender, RoutedEventArgs e) => ChangeAttribute("Strength", false);
            addDexterity.Click += (object sender, RoutedEventArgs e) => ChangeAttribute("Dexterity", true);
            subtractDexterity.Click += (object sender, RoutedEventArgs e) => ChangeAttribute("Dexterity", false);
            addIntelligence.Click += (object sender, RoutedEventArgs e) => ChangeAttribute("Intelligence", true);
            subtractIntelligence.Click += (object sender, RoutedEventArgs e) => ChangeAttribute("Intelligence", false);
            
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
            int currentDexterity = int.Parse(dexterityNumber.Text);
            int currentConstitution = int.Parse(constitutionNumber.Text);
            int currentIntelligence = int.Parse(intelligenceNumber.Text);
            int currentHitPoints = int.Parse(hitPointsNumber.Text);
            int currentWill = int.Parse(willNumber.Text);
            int currentPerception = int.Parse(perceptionNumber.Text);
            int currentBasicLift = int.Parse(basicLiftNumber.Text);
            decimal currentSpeed = int.Parse(speedNumber.Text);
            int currentMove = int.Parse(moveNumber.Text);
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
                    if (currentCharacterPoints >= 20 && add)
                    {
                        currentDexterity += 1;
                        currentSpeed = (currentDexterity + currentConstitution) / 4;
                        currentMove = (int)Math.Round(currentSpeed);
                        currentCharacterPoints -= 20;
                    }
                    else if (currentCharacterPoints >= 20 && !add && currentDexterity > 3)
                    {
                        currentDexterity -= 1;
                        currentSpeed = (currentDexterity + currentConstitution) / 4;
                        currentMove = (int)Math.Round(currentSpeed);
                        currentCharacterPoints += 20;
                    }
                    dexterityNumber.Text = currentDexterity.ToString();
                    characterPoints.Text = currentCharacterPoints.ToString();
                    speedNumber.Text = currentSpeed.ToString();
                    moveNumber.Text = currentMove.ToString();
                    break;
                case "Intelligence":
                    if (currentCharacterPoints >= 20 && add)
                    {
                        currentIntelligence += 1;
                        currentWill += 1;
                        currentPerception += 1;
                        currentCharacterPoints -= 20;
                    }
                    intelligenceNumber.Text = currentIntelligence.ToString();
                    characterPoints.Text = currentCharacterPoints.ToString();
                    willNumber.Text = currentWill.ToString();
                    perceptionNumber.Text = currentPerception.ToString();
                    break;
            }
        }
    }
}
