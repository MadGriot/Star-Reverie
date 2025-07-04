using System.Collections.Generic;
using System.Linq;
using Stride.Input;
using Stride.Engine;
using Stride.UI.Controls;
using StarReverieCore;
using StarReverieCore.Models;
using Stride.UI;
using Stride.UI.Panels;
using Stride.UI.Events;
using Microsoft.EntityFrameworkCore;

namespace Star_Reverie
{
    public class CharacterSelection : SyncScript
    {
        public UIPage CharacterSelectionPage;
        public Prefab CharacterSelectionButton;
        public UIPage CharacterSheet;
        private Entity uiEntity;
        private UIComponent uIComponent;
        private Entity charaterSheetEntity;
        private UIComponent characterSheetComponent;
        StarReverieDbContext starReverieDbContext = new StarReverieDbContext();

        private bool OnScreen;
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

            characters = starReverieDbContext.Characters
                .Include(c => c.AttributeScore)
                .ToList();
            characterList = CharacterSelectionPage.RootElement.FindVisualChildOfType<StackPanel>("CharacterList");
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

        public override void Update()
        {
            if (Input.IsKeyPressed(Keys.L))
            {
                if (!OnScreen)
                {
                    SceneSystem.SceneInstance.RootScene.Entities.Add(uiEntity);
                    OnScreen = true;
                }

            }
        }

        private void ReadCharacter(Character character)
        {
            SceneSystem.SceneInstance.RootScene.Entities.Remove(uiEntity);
            OnScreen = false;

            if (!CharacterSheetOnScreen)
            {
                
                characterSheetComponent.Page.RootElement.FindVisualChildOfType<TextBlock>("FirstName").Text = character.FirstName;
                characterSheetComponent.Page.RootElement.FindVisualChildOfType<TextBlock>("LastName").Text = character.LastName;
                characterSheetComponent.Page.RootElement.FindVisualChildOfType<TextBlock>("AgeNumber").Text = character.Age.ToString();
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
                SceneSystem.SceneInstance.RootScene.Entities.Add(charaterSheetEntity);
                CharacterSheetOnScreen = true;
            }
            
        }
    }
}
