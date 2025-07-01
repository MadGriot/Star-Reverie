using Stride.Input;
using Stride.Engine;

namespace Star_Reverie
{
    public class CharacterCreation : SyncScript
    {
        public UIPage CharacterCreationMain;
        private Entity uiEntity;
        private UIComponent uIComponent;
        private bool OnScreen = false;

        public override void Start()
        {
            uiEntity = new();
            uIComponent = new();
            uIComponent.Page = CharacterCreationMain;
            uiEntity.Add(uIComponent);
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
    }
}
