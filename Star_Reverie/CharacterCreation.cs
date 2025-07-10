using Stride.Input;
using Stride.Engine;
using Stride.UI.Controls;
using Stride.UI;
using Stride.UI.Events;
using System;
using StarReverieCore.Models;
using StarReverieCore;
using Star_Reverie.Globals;
using Stride.UI.Panels;
using StarReverieCore.Mechanics;
using System.Linq;

namespace Star_Reverie
{
    public class CharacterCreation : SyncScript
    {
        public UIPage CharacterCreationMain;
        public int AttributeMin = 3;
        public Prefab TextBlockPrefab;
        public Prefab SkillListPrefab;
        public Prefab SelectionButton;
        private Entity uiEntity;
        private UIComponent uIComponent;
        private bool OnScreen = false;

        //TextBlocks
        TextBlock characterPoints;
        EditText firstName;
        //EditText middleName;
        EditText lastName;
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
        TextBlock dodgeNumber;

        public override void Start()
        {
            uiEntity = new();
            uIComponent = new();
            uIComponent.Page = CharacterCreationMain;
            uiEntity.Add(uIComponent);
            StatNumberMapping();
            ButtonEventMapping();
            SkillList();
        }

        public override void Update()
        {
            if (Input.IsKeyPressed(Keys.C))
            {
                if (!OnScreen && CurrentGameState.GameState != GameState.Dialogue)
                {
                    SceneSystem.SceneInstance.RootScene.Entities.Add(uiEntity);
                    OnScreen = true;
                }

            }
        }
        private void StatNumberMapping()
        {
            characterPoints = CharacterCreationMain.RootElement.FindVisualChildOfType<TextBlock>("CharacterPointsNumber");
            firstName = CharacterCreationMain.RootElement.FindVisualChildOfType<EditText>("FirstName");
            lastName = CharacterCreationMain.RootElement.FindVisualChildOfType<EditText>("LastName");
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
            dodgeNumber = CharacterCreationMain.RootElement.FindVisualChildOfType<TextBlock>("DodgeNumber");
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
            Button addConstitution = CharacterCreationMain.RootElement.FindVisualChildOfType<Button>("AddConstitution");
            Button subtractConstitution = CharacterCreationMain.RootElement.FindVisualChildOfType<Button>("SubtractConstitution");
            Button addHitPoints = CharacterCreationMain.RootElement.FindVisualChildOfType<Button>("AddHitPoints");
            Button subtractHitPoints = CharacterCreationMain.RootElement.FindVisualChildOfType<Button>("SubtractHitPoints");
            Button addWill = CharacterCreationMain.RootElement.FindVisualChildOfType<Button>("AddWill");
            Button subtractWill = CharacterCreationMain.RootElement.FindVisualChildOfType<Button>("SubtractWill");
            Button addPerception = CharacterCreationMain.RootElement.FindVisualChildOfType<Button>("AddPerception");
            Button subtractPerception = CharacterCreationMain.RootElement.FindVisualChildOfType<Button>("SubtractPerception");
            Button addStamina = CharacterCreationMain.RootElement.FindVisualChildOfType<Button>("AddStamina");
            Button subtractStamina = CharacterCreationMain.RootElement.FindVisualChildOfType<Button>("SubtractStamina");
            //Button addSpeed = CharacterCreationMain.RootElement.FindVisualChildOfType<Button>("AddSpeed");
            //Button subtractSpeed = CharacterCreationMain.RootElement.FindVisualChildOfType<Button>("SubtractSpeed");
            //Button addMove = CharacterCreationMain.RootElement.FindVisualChildOfType<Button>("AddMove");
            //Button subtractMove = CharacterCreationMain.RootElement.FindVisualChildOfType<Button>("SubtractMove");
            Button saveButton = CharacterCreationMain.RootElement.FindVisualChildOfType<Button>("Save");

            addAge.Click += (object sender, RoutedEventArgs e) => ChangeAge(true);
            subtractAge.Click += (object sender, RoutedEventArgs e) => ChangeAge(false);
            addStrength.Click += (object sender, RoutedEventArgs e) => ChangeAttribute("Strength", true);
            subtractStrength.Click += (object sender, RoutedEventArgs e) => ChangeAttribute("Strength", false);
            addDexterity.Click += (object sender, RoutedEventArgs e) => ChangeAttribute("Dexterity", true);
            subtractDexterity.Click += (object sender, RoutedEventArgs e) => ChangeAttribute("Dexterity", false);
            addIntelligence.Click += (object sender, RoutedEventArgs e) => ChangeAttribute("Intelligence", true);
            subtractIntelligence.Click += (object sender, RoutedEventArgs e) => ChangeAttribute("Intelligence", false);
            addConstitution.Click += (object sender, RoutedEventArgs e) => ChangeAttribute("Constitution", true);
            subtractConstitution.Click += (object sender, RoutedEventArgs e) => ChangeAttribute("Constitution", false);
            addHitPoints.Click += (object sender, RoutedEventArgs e) => ChangeAttribute("HitPoints", true);
            subtractHitPoints.Click += (object sender, RoutedEventArgs e) => ChangeAttribute("HitPoints", false);
            addWill.Click += (object sender, RoutedEventArgs e) => ChangeAttribute("Will", true);
            subtractWill.Click += (object sender, RoutedEventArgs e) => ChangeAttribute("Will", false);
            addPerception.Click += (object sender, RoutedEventArgs e) => ChangeAttribute("Perception", true);
            subtractPerception.Click += (object sender, RoutedEventArgs e) => ChangeAttribute("Perception", false);
            addStamina.Click += (object sender, RoutedEventArgs e) => ChangeAttribute("Stamina", true);
            subtractStamina.Click += (object sender, RoutedEventArgs e) => ChangeAttribute("Stamina", false);
            //addSpeed.Click += (object sender, RoutedEventArgs e) => ChangeAttribute("Speed", true);
            //subtractSpeed.Click += (object sender, RoutedEventArgs e) => ChangeAttribute("Speed", false);
            //addMove.Click += (object sender, RoutedEventArgs e) => ChangeAttribute("Move", true);
            //subtractMove.Click += (object sender, RoutedEventArgs e) => ChangeAttribute("Move", false);
            saveButton.Click += (object sender, RoutedEventArgs e) => SaveCharacter();
            
        }

        private void SkillList()
        {
            StackPanel skillList = CharacterCreationMain
                .RootElement
                .FindVisualChildOfType<StackPanel>("SkillList");
            StarReverieDbContext starReverieDbContext = new StarReverieDbContext();

            foreach (string skill in Enum.GetNames(typeof(Skill)))
            {
                Enum.TryParse(skill, out Skill result);
                StackPanel skillListPanel = (StackPanel)SkillListPrefab
                            .Instantiate()
                            .First()
                            .Get<UIComponent>().Page
                            .RootElement;
                TextBlock textBlock = (TextBlock)TextBlockPrefab
                            .Instantiate()
                            .First()
                            .Get<UIComponent>().Page
                            .RootElement;
                Button minusButton = (Button)SelectionButton
                    .Instantiate()
                    .First()
                    .Get<UIComponent>().Page
                    .RootElement;
                minusButton.FindVisualChildOfType<TextBlock>().Text = "-";

                Button plusButton = (Button)SelectionButton
                    .Instantiate()
                    .First()
                    .Get<UIComponent>().Page
                    .RootElement;
                plusButton.FindVisualChildOfType<TextBlock>().Text = "+";
                TextBlock numBlock = (TextBlock)TextBlockPrefab
                            .Instantiate()
                            .First()
                            .Get<UIComponent>().Page
                            .RootElement;
                textBlock.Text = skill;
                plusButton.Click += (object sender, RoutedEventArgs e) => ChangeSkill(numBlock, result, true);
                minusButton.Click += (object sender, RoutedEventArgs e) => ChangeSkill(numBlock, result, false);
                //starReverieDbContext.Skills.First(x => result == x.Skill).Level.ToString();
                numBlock.Text = "0";
                skillListPanel.Children.Add(minusButton);
                skillListPanel.Children.Add(textBlock);
                skillListPanel.Children.Add(plusButton);
                skillListPanel.Children.Add(numBlock);
                skillList.Children.Add(skillListPanel);
            }
            
        }

        private void ChangeSkill(TextBlock textBlock, Skill skill, bool add)
        {
            int currentCharacterPoints = int.Parse(characterPoints.Text);
            int currentSkillLevel = int.Parse(textBlock.Text);
            int skillCost = SkillMechanics.GetSkillCost(currentSkillLevel);
            if (currentCharacterPoints >= skillCost  && add)
            {
                textBlock.Text = (currentSkillLevel + 1).ToString();
                currentCharacterPoints -= skillCost;
                characterPoints.Text = currentCharacterPoints.ToString();
            }
            else if (!add && int.Parse(textBlock.Text) > 0)
            {
                skillCost = SkillMechanics.GetSkillCost(currentSkillLevel-1);
                textBlock.Text = (currentSkillLevel - 1).ToString();
                currentCharacterPoints += skillCost;
                characterPoints.Text = currentCharacterPoints.ToString();
            }
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
            int currentStamina = int.Parse(staminaNumber.Text);
            int currentBasicLift = int.Parse(basicLiftNumber.Text);
            decimal currentSpeed = decimal.Parse(speedNumber.Text);
            int currentMove = int.Parse(moveNumber.Text);
            int currentDodge = int.Parse(dodgeNumber.Text);

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
                    else if (currentCharacterPoints >= 0 && !add && currentStrength > AttributeMin && currentHitPoints > AttributeMin)
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
                        currentSpeed = (decimal)(currentDexterity + currentConstitution) / 4;
                        currentMove = (int)Math.Round(currentSpeed);
                        currentDodge = currentMove + 3;
                        currentCharacterPoints -= 20;
                    }
                    else if (currentCharacterPoints >= 0 && !add && currentDexterity > AttributeMin)
                    {
                        currentDexterity -= 1;
                        currentSpeed = (decimal)(currentDexterity + currentConstitution) / 4;
                        currentMove = (int)Math.Round(currentSpeed);
                        currentDodge = currentMove + 3;
                        currentCharacterPoints += 20;
                    }
                    dexterityNumber.Text = currentDexterity.ToString();
                    characterPoints.Text = currentCharacterPoints.ToString();
                    dodgeNumber.Text = currentDodge.ToString();
                    speedNumber.Text = currentSpeed.ToString("F2");
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
                    else if (currentCharacterPoints >= 0 && !add && currentIntelligence > 6 && currentWill > 6 && currentPerception > 6)
                    {
                        currentIntelligence -= 1;
                        currentWill -= 1;
                        currentPerception -= 1;
                        currentCharacterPoints += 20;
                    }
                    intelligenceNumber.Text = currentIntelligence.ToString();
                    characterPoints.Text = currentCharacterPoints.ToString();
                    willNumber.Text = currentWill.ToString();
                    perceptionNumber.Text = currentPerception.ToString();
                    break;
                case "Constitution":
                    if (currentCharacterPoints >= 10 && add)
                    {
                        currentConstitution += 1;
                        currentStamina += 1;
                        currentSpeed = (decimal)(currentDexterity + currentConstitution) / 4;
                        currentMove = (int)Math.Round(currentSpeed);
                        currentDodge = currentMove + 3;
                        currentCharacterPoints -= 10;
                    }
                    else if (currentCharacterPoints >= 0 && !add && currentConstitution > AttributeMin && currentStamina > AttributeMin)
                    {
                        currentConstitution -= 1;
                        currentStamina -= 1;
                        currentSpeed = (decimal)(currentDexterity + currentConstitution) / 4;
                        currentMove = (int)Math.Round(currentSpeed);
                        currentDodge = currentMove + 3;
                        currentCharacterPoints += 10;
                    }
                    constitutionNumber.Text = currentConstitution.ToString();
                    characterPoints.Text = currentCharacterPoints.ToString();
                    staminaNumber.Text = currentStamina.ToString();
                    speedNumber.Text = currentSpeed.ToString("F2");
                    moveNumber.Text = currentMove.ToString();
                    dodgeNumber.Text = currentDodge.ToString();
                    characterPoints.Text = currentCharacterPoints.ToString();
                    break;
                case "HitPoints":
                    if (currentCharacterPoints >= 2 && add)
                    {
                        currentHitPoints += 1;
                        currentCharacterPoints -= 2;
                    }
                    else if (currentCharacterPoints >= 0 && !add && currentHitPoints > currentStrength)
                    {
                        currentHitPoints -= 1;
                        currentCharacterPoints += 2;
                    }
                    hitPointsNumber.Text = currentHitPoints.ToString();
                    characterPoints.Text = currentCharacterPoints.ToString();
                    break;
                case "Will":
                    if (currentCharacterPoints >= 5 && add)
                    {
                        currentWill += 1;
                        currentCharacterPoints -= 5;
                    }
                    else if (currentCharacterPoints >= 0 && !add && currentWill > currentIntelligence)
                    {
                        currentWill -= 1;
                        currentCharacterPoints += 5;
                    }
                    willNumber.Text = currentWill.ToString();
                    characterPoints.Text = currentCharacterPoints.ToString();
                    break;

                case "Perception":
                    if (currentCharacterPoints >= 5 && add)
                    {
                        currentPerception += 1;
                        currentCharacterPoints -= 5;
                    }
                    else if (currentCharacterPoints >= 0 && !add && currentPerception > currentIntelligence)
                    {
                        currentPerception -= 1;
                        currentCharacterPoints += 5;
                    }
                    perceptionNumber.Text = currentPerception.ToString();
                    characterPoints.Text = currentCharacterPoints.ToString();
                    break;

                case "Stamina":
                    if (currentCharacterPoints >= 3 && add)
                    {
                        currentStamina += 1;
                        currentCharacterPoints -= 3;
                    }
                    else if (currentCharacterPoints >= 0 && !add && currentStamina > currentConstitution)
                    {
                        currentStamina -= 1;
                        currentCharacterPoints += 3;
                    }
                    staminaNumber.Text = currentStamina.ToString();
                    characterPoints.Text = currentCharacterPoints.ToString();
                    break;

                //case "Speed":
                //    if (currentCharacterPoints >= 5 && add)
                //    {
                //        currentSpeed += 0.25m;
                //        currentCharacterPoints -= 5;
                //    }
                //    else if (currentCharacterPoints >= 0 && !add && currentSpeed >
                //        (decimal)(currentDexterity + currentConstitution) / 4)
                //    {
                //        currentSpeed -= 0.25m;
                //        currentCharacterPoints += 5;
                //    }
                //    speedNumber.Text = currentSpeed.ToString();
                //    characterPoints.Text = currentCharacterPoints.ToString();
                //    break;
            }
        }

        private void SaveCharacter()
        {
            StarReverieDbContext starReverieDbContext = new StarReverieDbContext();
            Character character = new Character
            {
                FirstName = firstName.Text,
                LastName = lastName.Text,
                Age = int.Parse(ageNumber.Text),
                Level = 1,
                PowerLevel = 100,
            };

            starReverieDbContext.Characters.Add(character);
            starReverieDbContext.SaveChanges();

            AttributeScore attributeScore = new AttributeScore
            {
                Strength = int.Parse(strengthNumber.Text),
                Dexterity = int.Parse(dexterityNumber.Text),
                Intelligence = int.Parse(intelligenceNumber.Text),
                Constitution = int.Parse(constitutionNumber.Text),
                HP = int.Parse(hitPointsNumber.Text),
                Will = int.Parse(willNumber.Text),
                Perception = int.Parse(perceptionNumber.Text),
                Stamina = int.Parse(staminaNumber.Text),
                BasicLift = int.Parse(basicLiftNumber.Text),
                Speed = decimal.Parse(speedNumber.Text),
                BasicMove = int.Parse(moveNumber.Text),
                Dodge = int.Parse(dodgeNumber.Text),
                XP = 1000,
                CharacterId = character.Id
            };

            starReverieDbContext.AttributeScores.Add(attributeScore);
            starReverieDbContext.SaveChanges();

            if (OnScreen)
            {
                SceneSystem.SceneInstance.RootScene.Entities.Remove(uiEntity);
                uiEntity.Dispose();
                OnScreen = false;
            }
        }
    }
}
