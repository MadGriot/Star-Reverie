using System.Collections.Generic;
using System.Linq;
using Stride.Core.Mathematics;
using Stride.Input;
using Stride.Engine;
using Stride.UI.Controls;
using StarReverieCore;
using StarReverieCore.Models;
using Stride.UI;
using Stride.UI.Panels;
using Stride.Graphics;
using Stride.Core.Shaders.Ast;

namespace Star_Reverie
{
    public class CharacterSelection : SyncScript
    {
        public UIPage CharacterSelectionPage;
        public Prefab CharacterSelectionButton;
        private Entity uiEntity;
        private UIComponent uIComponent;
        StarReverieDbContext starReverieDbContext = new StarReverieDbContext();

        private bool OnScreen;
        private List<Character> characters;
        private StackPanel characterList;
        public override void Start()
        {
            uiEntity = new();
            uIComponent = new();
            uIComponent.Page = CharacterSelectionPage;
            uiEntity.Add(uIComponent);

            characters = starReverieDbContext.Characters.ToList();
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
    }
}
