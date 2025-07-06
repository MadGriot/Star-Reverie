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
                UIEntity = new();
                UIComponent = new UIComponent { Page = DialogueSelectionPage };
                UIEntity.Add(UIComponent);
                CurrentGameState.GameState = GameState.Dialogue;
                DialoguePageOnScreen = true;
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
