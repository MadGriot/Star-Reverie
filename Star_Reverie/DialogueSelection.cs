using System;
using System.Collections.Generic;
using System.Linq;
using Stride.Core.Mathematics;
using Stride.Input;
using Stride.Engine;
using Star_Reverie.Globals;
using StarReverieCore.Models;
using StarReverieCore;
using Stride.UI.Panels;
using Stride.UI;
using Stride.UI.Controls;
using Stride.UI.Events;
using Stride.Core.Extensions;

namespace Star_Reverie
{
    public class DialogueSelection : SyncScript
    {
        public UIPage DialogueSelectionPage;
        public Prefab DialogueSelectionButton;
        public Prefab ResponseTextBlock;
        public UIPage DialoguePage;
        public UIPage DialogueResponsePage;

        private Entity UIEntity;
        private UIComponent UIComponent;
        private bool DialoguePageOnScreen;
        private bool DialogueDetailsPageOnScreen;
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
                    button.Click += (object sender, RoutedEventArgs e) => ReadDialogue(dialgoue);
                    dialgoueList.Children.Add(button);

                }
                CurrentGameState.GameState = GameState.Dialogue;
                DialoguePageOnScreen = true;
                SceneSystem.SceneInstance.RootScene.Entities.Add(UIEntity);
            }
        }

        private void ReadDialogue(DialogueModel dialogue)
        {


            if (!DialogueDetailsPageOnScreen)
            {
                if (DialoguePageOnScreen)
                {
                    SceneSystem.SceneInstance.RootScene.Entities.Remove(UIEntity);
                    UIEntity.Dispose();
                    DialoguePageOnScreen = false;

                }
                UIEntity = new();
                UIComponent = new UIComponent { Page = DialoguePage };
                UIComponent.Page.RootElement.FindVisualChildOfType<TextBlock>("SpeakerFirstName").Text = dialogue.SpeakerFirstName;
                UIComponent.Page.RootElement.FindVisualChildOfType<TextBlock>("SpeakerLastName").Text = dialogue.SpeakerLastName;
                UIComponent.Page.RootElement.FindVisualChildOfType<TextBlock>("SpeakerDialogue").Text = dialogue.SpeakerDialogue;

                if (!dialogue.Responses.IsNullOrEmpty())
                {
                    foreach (DialogueModel response in dialogue.Responses)
                    {
                        TextBlock textBlock = (TextBlock)ResponseTextBlock
                            .Instantiate()
                            .First()
                            .Get<UIComponent>().Page
                            .RootElement;
                        textBlock.Text = response.SpeakerDialogue;
                        UIComponent.Page.RootElement.FindVisualChildOfType<StackPanel>("Responses")
                            .Children.Add(textBlock);

                    }
                }

                UIComponent.Page.RootElement.FindVisualChildOfType<Button>("AddResponse").Click += 
                    (object sender, RoutedEventArgs e) => ShowResponseEditor(dialogue);
                UIComponent.Page.RootElement.FindVisualChildOfType<Button>("Delete")
                    .Click += (object sender, RoutedEventArgs e) => DeleteDialogue(dialogue);
                UIEntity.Add(UIComponent);


                CurrentGameState.GameState = GameState.Dialogue;
                DialogueDetailsPageOnScreen = true;
                SceneSystem.SceneInstance.RootScene.Entities.Add(UIEntity);

            }
        }
        private void ShowResponseEditor(DialogueModel dialogue)
        {
            if (!DialgoueResponseOnScreen)
            {
                if (DialoguePageOnScreen || DialogueDetailsPageOnScreen)
                {
                    SceneSystem.SceneInstance.RootScene.Entities.Remove(UIEntity);
                    UIEntity.Dispose();
                    DialoguePageOnScreen = false;
                    DialogueDetailsPageOnScreen = false;

                }


                UIEntity = new();
                UIComponent = new UIComponent { Page = DialogueResponsePage };
                UIComponent.Page.RootElement.FindVisualChildOfType<TextBlock>("SpeakerFirstName").Text = dialogue.SpeakerFirstName;
                UIComponent.Page.RootElement.FindVisualChildOfType<TextBlock>("SpeakerLastName").Text = dialogue.SpeakerLastName;
                UIComponent.Page.RootElement.FindVisualChildOfType<TextBlock>("SpeakerDialogue").Text = dialogue.SpeakerDialogue;
                UIComponent.Page.RootElement.FindVisualChildOfType<Button>("Save")
                    .Click += (object sender, RoutedEventArgs e) => AddResponse(dialogue);
                UIEntity.Add(UIComponent);
                SceneSystem.SceneInstance.RootScene.Entities.Add(UIEntity);
                DialoguePageOnScreen = true;

            }
        }

        private void AddResponse(DialogueModel dialogue)
        {
            StarReverieDbContext starReverieDbContext = new StarReverieDbContext();
            DialogueModel response = new DialogueModel
            {
                SpeakerFirstName = UIComponent.Page.RootElement.FindVisualChildOfType<EditText>("ResponderFirstName").Text,
                SpeakerLastName = UIComponent.Page.RootElement.FindVisualChildOfType<EditText>("ResponderLastName").Text,
                SpeakerDialogue = UIComponent.Page.RootElement.FindVisualChildOfType<EditText>("ResponderDialogue").Text,
                ParentDialogueId = dialogue.Id,

            };
            starReverieDbContext.Add(response);
            starReverieDbContext.SaveChanges();

            SceneSystem.SceneInstance.RootScene.Entities.Remove(UIEntity);
            UIEntity.Dispose();
            DialgoueResponseOnScreen = false;
            CurrentGameState.GameState = GameState.Exploration;

        }

        private void DeleteDialogue(DialogueModel dialogue)
        {
            StarReverieDbContext starReverieDbContext = new StarReverieDbContext();


            SceneSystem.SceneInstance.RootScene.Entities.Remove(UIEntity);
            UIEntity.Dispose();
            DialgoueResponseOnScreen = false;
            CurrentGameState.GameState = GameState.Exploration;

            starReverieDbContext.Remove(dialogue);
            starReverieDbContext.SaveChanges();
        }
    }
}
