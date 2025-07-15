using Stride.Engine;
using Stride.Rendering;

namespace Star_Reverie
{
    public class GridSystemVisualSingle : SyncScript
    {
        public Material BlankMaterial;
        public Material Material;
        public Entity Cell;

        public void Show()
        {
            Cell.Get<ModelComponent>().Materials.Remove(0);
            Cell.Get<ModelComponent>().Materials.Add(0, Material);
        }

        public void Hide()
        {
            Cell.Get<ModelComponent>().Materials.Remove(0);
            Cell.Get<ModelComponent>().Materials.Add(0, BlankMaterial);
        }

        public override void Update()
        {
            // Do stuff every new frame
        }
    }
}
