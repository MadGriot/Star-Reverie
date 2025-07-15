using Stride.Engine;

namespace Star_Reverie
{
    public class Testing : SyncScript
    {
        public Entity GridDebugObject;

        private GridSystem gridSystem;
        public override void Start()
        {
            gridSystem = new GridSystem(10, 1, 10, 2f);
            gridSystem.CreateDebugObjects(GridDebugObject);
        }

        public override void Update()
        {
            // Do stuff every new frame
        }
    }
}
