using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stride.Core.Mathematics;
using Stride.Input;
using Stride.Engine;
using System.Security.Principal;
using Star_Reverie.Globals;
using StarReverieCore.Models;
using StarReverieCore;
using System.Windows.Media.TextFormatting;
using Stride.UI.Panels;
using Stride.UI;
using Stride.UI.Controls;
using Stride.UI.Events;

namespace Star_Reverie
{
    public class DialogueSelection : SyncScript
    {
        public UIPage DialogueSelectionPage;
        public Prefab DialogueSelectionButton;
        public UIPage DialoguePage;
        public UIPage DialogueResponsePage;

        private Entity UIEntity;
        private UIComponent UIComponent;
        private bool DialoguePageOnScreen;
        private bool DialgoueResponseOnScreen;
        private List<DialogueModel> dialogues;
        private StackPanel dialgoueList;
        public override void Start()
        {
            // Initialization of the script.
        }

        public override void Update()
        {
            if (Input.IsKeyPressed(Keys.D0))
            {
                ShowDialogueList();
            }
        }
        private void ShowDialogueList()
        {

            if (!DialoguePageOnScreen)
            {
                StarReverieDbContext starReverieDbContext = new StarReverieDbContext();
                UIEntity = new();
                UIComponent = new UIComponent { Page = DialogueSelectionPage };
                UIEntity.Add(UIComponent);

                dialogues = starReverieDbContext.Dialogues
                    .ToList();

                dialgoueList = DialogueSelectionPage.RootElement.FindVisualChildOfType<StackPanel>("DialogueList");
                dialgoueList.Children.Clear();
                foreach (DialogueModel dialgoue in dialogues)
                {
                    Button button = (Button)DialogueSelectionButton
                        .Instantiate()
                        .First()
                        .Get<UIComponent>().Page
                        .RootElement;
                    button.Name = dialgoue.SpeakerFirstName;
                    button.FindVisualChildOfType<TextBlock>().Text = $"{dialgoue.SpeakerFirstName} {dialgoue.SpeakerLastName}";
                    button.HorizontalAlignment = HorizontalAlignment.Center;
                    dialgoueList.Children.Add(button);

                   // button.Click += (object sender, RoutedEventArgs e) => ReadCharacter(character);
                }
                CurrentGameState.GameState = GameState.Dialogue;
                DialoguePageOnScreen = true;
                SceneSystem.SceneInstance.RootScene.Entities.Add(UIEntity);
            }
        }

        private void ShowResponseEditor()
        {
            if (!DialgoueResponseOnScreen)
            {
                if (DialoguePageOnScreen)
                {
                    SceneSystem.SceneInstance.RootScene.Entities.Remove(UIEntity);
                    UIEntity.Dispose();
                    DialoguePageOnScreen = false;


                }
                UIEntity = new();
                UIComponent = new UIComponent { Page = DialogueResponsePage };
                UIEntity.Add(UIComponent);
                SceneSystem.SceneInstance.RootScene.Entities.Add(UIEntity);
                DialoguePageOnScreen = true;

            }
        }
    }
}
