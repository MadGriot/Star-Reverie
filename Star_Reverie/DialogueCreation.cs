using Stride.Input;
using Stride.Engine;
using StarReverieCore;
using StarReverieCore.Models;
using Stride.UI;
using Stride.UI.Controls;
using Stride.UI.Events;
using Star_Reverie.Globals;

namespace Star_Reverie
{
    public class DialogueCreation : SyncScript
    {
        public UIPage DialogueCreationMain;
        public UIPage DialogueCreationResponse;
        private Entity UIEntity;
        private UIComponent UIComponent;

        private bool DialogueCreationOnScreen;


        public override void Start()
        {

        }

        public override void Update()
        {
            if (Input.IsKeyPressed(Keys.P))
            {
                if (!DialogueCreationOnScreen && CurrentGameState.GameState != GameState.Dialogue)
                {
                    CurrentGameState.GameState = GameState.Dialogue;
                    UIEntity = new();
                    UIComponent = new UIComponent { Page = DialogueCreationMain };
                    UIEntity.Add(UIComponent);
                    ButtonEventMapping();
                    SceneSystem.SceneInstance.RootScene.Entities.Add(UIEntity);
                    DialogueCreationOnScreen = true;
                }
            }
        }
        private void ButtonEventMapping()
        {
            DialogueCreationMain.RootElement.FindVisualChildOfType<Button>("Save").Click +=
                (object sender, RoutedEventArgs e) => SaveDialogue();

        }
        private void SaveDialogue()
        {
            StarReverieDbContext starReverieDbContext = new StarReverieDbContext();
            DialogueModel dialogue = new DialogueModel
            {
                SpeakerFirstName = DialogueCreationMain.RootElement.FindVisualChildOfType<EditText>("SpeakerFirstName").Text,
                SpeakerLastName = DialogueCreationMain.RootElement.FindVisualChildOfType<EditText>("SpeakerLastName").Text,
                SpeakerDialogue = DialogueCreationMain.RootElement.FindVisualChildOfType<EditText>("SpeakerDialogue").Text
            };

            starReverieDbContext.Dialogues.Add(dialogue);
            starReverieDbContext.SaveChanges();

            if (DialogueCreationOnScreen)
            {
                SceneSystem.SceneInstance.RootScene.Entities.Remove(UIEntity);
                UIEntity.Dispose();
                DialogueCreationOnScreen = false;
                CurrentGameState.GameState = GameState.Exploration;

            }

        }


    }
}
