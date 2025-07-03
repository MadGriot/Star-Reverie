using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stride.Core.Mathematics;
using Stride.Input;
using Stride.Engine;
using Stride.UI.Controls;
using StarReverieCore;
using StarReverieCore.Models;
using Stride.UI;
using Stride.UI.Panels;
using Stride.Graphics;

namespace Star_Reverie
{
    public class CharacterSelection : SyncScript
    {
        public UIPage CharacterSelectionPage;
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
                TextBlock textBlock = new TextBlock
                {
                    Name = character.FirstName,

                        Text = $"{character.FirstName} {character.LastName}",
                        Font = Content.Load<SpriteFont>("StrideDefaultFont"),
                        TextSize = 20,
                        TextColor = Color.White,
                        OutlineColor = Color.Black,
                    HorizontalAlignment = HorizontalAlignment.Center,
                };
                characterList.Children.Add(textBlock);
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
