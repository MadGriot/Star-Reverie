using Microsoft.EntityFrameworkCore;
using Star_Reverie.Globals;
using StarReverieCore;
using StarReverieCore.Mechanics;
using StarReverieCore.Models;
using Stride.Core.Extensions;
using Stride.Engine;
using Stride.Input;
using Stride.UI;
using Stride.UI.Controls;
using Stride.UI.Events;
using Stride.UI.Panels;
using System.Collections.Generic;
using System.Linq;

namespace Star_Reverie
{
    public class CharacterSelection : SyncScript
    {
        public UIPage CharacterSelectionPage;
        public Prefab CharacterSelectionButton;
        public Prefab TextBlockPrefab;
        public Prefab SkillListPrefab;
        public UIPage CharacterSheet;
        private Entity uiEntity;
        private UIComponent uIComponent;
        private Entity charaterSheetEntity;
        private UIComponent characterSheetComponent;
        StarReverieDbContext starReverieDbContext = new StarReverieDbContext();

        private bool CharacterListOnScreen;
        private bool CharacterSheetOnScreen;
        private List<Character> characters;
        private StackPanel characterList;
        public override void Start()
        {
            uiEntity = new();
            uIComponent = new();
            uIComponent.Page = CharacterSelectionPage;
            uiEntity.Add(uIComponent);
            charaterSheetEntity = new();
            characterSheetComponent = new UIComponent { Page = CharacterSheet };
            charaterSheetEntity.Add(characterSheetComponent);

            UpdateList();

        }

        public override void Update()
        {
            if (Input.IsKeyPressed(Keys.L))
            {
                if (!CharacterListOnScreen && CurrentGameState.GameState != GameState.Dialogue)
                {
                    SceneSystem.SceneInstance.RootScene.Entities.Add(uiEntity);
                    CharacterListOnScreen = true;
                }

            }
        }

        private void ReadCharacter(Character character)
        {
            SceneSystem.SceneInstance.RootScene.Entities.Remove(uiEntity);
            CharacterListOnScreen = false;

            if (!CharacterSheetOnScreen)
            {
                
                characterSheetComponent.Page.RootElement.FindVisualChildOfType<TextBlock>("FirstName").Text = character.FirstName;
                characterSheetComponent.Page.RootElement.FindVisualChildOfType<TextBlock>("LastName").Text = character.LastName;
                characterSheetComponent.Page.RootElement.FindVisualChildOfType<TextBlock>("AgeNumber").Text = character.Age.ToString();
                characterSheetComponent.Page.RootElement.FindVisualChildOfType<TextBlock>("LevelNumber").Text = character.Level.ToString();
                characterSheetComponent.Page.RootElement.FindVisualChildOfType<TextBlock>("XPNumber").Text = character.AttributeScore.XP.ToString();
                characterSheetComponent.Page.RootElement.FindVisualChildOfType<TextBlock>("StrengthNumber").Text = character.AttributeScore.Strength.ToString();
                characterSheetComponent.Page.RootElement.FindVisualChildOfType<TextBlock>("DexterityNumber").Text = character.AttributeScore.Dexterity.ToString();
                characterSheetComponent.Page.RootElement.FindVisualChildOfType<TextBlock>("ConstitutionNumber").Text = character.AttributeScore.Constitution.ToString();
                characterSheetComponent.Page.RootElement.FindVisualChildOfType<TextBlock>("HitPointsNumber").Text = character.AttributeScore.HP.ToString();
                characterSheetComponent.Page.RootElement.FindVisualChildOfType<TextBlock>("WillNumber").Text = character.AttributeScore.Will.ToString();
                characterSheetComponent.Page.RootElement.FindVisualChildOfType<TextBlock>("PerceptionNumber").Text = character.AttributeScore.Perception.ToString();
                characterSheetComponent.Page.RootElement.FindVisualChildOfType<TextBlock>("StaminaNumber").Text = character.AttributeScore.Stamina.ToString();
                characterSheetComponent.Page.RootElement.FindVisualChildOfType<TextBlock>("BasicLiftNumber").Text = character.AttributeScore.BasicLift.ToString();
                characterSheetComponent.Page.RootElement.FindVisualChildOfType<TextBlock>("SpeedNumber").Text = character.AttributeScore.Speed.ToString();
                characterSheetComponent.Page.RootElement.FindVisualChildOfType<TextBlock>("DodgeNumber").Text = character.AttributeScore.Dodge.ToString();
                characterSheetComponent.Page.RootElement.FindVisualChildOfType<TextBlock>("EncumbranceStatus").Text = character.AttributeScore.Encumbrance.ToString();
                characterSheetComponent.Page.RootElement.FindVisualChildOfType<Button>("Delete").Click += (object sender, RoutedEventArgs e) => DeleteCharacter(character);

                StackPanel skillList = characterSheetComponent.Page.RootElement.FindVisualChildOfType<StackPanel>("SkillList");

                if (!character.Skills.IsNullOrEmpty())
                {
                    foreach (SkillModel skill in character.Skills)
                    {
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
                        TextBlock numBlock = (TextBlock)TextBlockPrefab
                                    .Instantiate()
                                    .First()
                                    .Get<UIComponent>().Page
                                    .RootElement;
                        textBlock.Text = skill.Skill.ToString();
                        numBlock.Text = SkillMechanics.GetSkillNumber(character.AttributeScore, skill).ToString();
                        skillListPanel.Children.Add(textBlock);
                        skillListPanel.Children.Add(numBlock);
                        skillList.Children.Add(skillListPanel);

                    }
                }



                SceneSystem.SceneInstance.RootScene.Entities.Add(charaterSheetEntity);
                CharacterSheetOnScreen = true;
            }
            
        }

        private void DeleteCharacter(Character character)
        {
            if (CharacterSheetOnScreen && !CharacterListOnScreen)
            {
                SceneSystem.SceneInstance.RootScene.Entities.Remove(charaterSheetEntity);
                starReverieDbContext.Remove(character);
                starReverieDbContext.SaveChangesAsync();
                CharacterSheetOnScreen = false;
                UpdateList();
                SceneSystem.SceneInstance.RootScene.Entities.Add(uiEntity);
                CharacterListOnScreen = true;

            }
        }

        private void UpdateList()
        {
            characters = starReverieDbContext.Characters
                        .Include(c => c.AttributeScore)
                        .Include(s => s.Skills)
                        .ToList();

            characterList = CharacterSelectionPage.RootElement.FindVisualChildOfType<StackPanel>("CharacterList");
            characterList.Children.Clear();
            foreach (Character character in characters)
            {
                Button button = (Button)CharacterSelectionButton
                    .Instantiate()
                    .First()
                    .Get<UIComponent>().Page
                    .RootElement;
                button.Name = character.FirstName;
                button.FindVisualChildOfType<TextBlock>().Text = $"{character.FirstName} {character.LastName}";
                button.HorizontalAlignment = HorizontalAlignment.Center;
                characterList.Children.Add(button);

                button.Click += (object sender, RoutedEventArgs e) => ReadCharacter(character);
            }
        }
    }
}
