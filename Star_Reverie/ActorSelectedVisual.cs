using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stride.Core.Mathematics;
using Stride.Input;
using Stride.Engine;
using Stride.Rendering;

namespace Star_Reverie
{
    public class ActorSelectedVisual : SyncScript
    {
        public Entity Actor;
        public Entity SelectionAsset;
        public Material BlankMaterial;
        public Material Material;

        public ActorActionSystem ActionSystem;

        public override void Start()
        {
            //ActionSystem = Entity.Scene.Entities
            //                .SelectMany(e => e.Components)
            //                .OfType<ActorActionSystem>()
            //                .FirstOrDefault();
            ActionSystem.OnSelectedActorChanged += ActorActionSytem_OnSelectedActorChanged;
            UpdateVisual();
        }

        public override void Update()
        {
            // Do stuff every new frame
        }

        private void ActorActionSytem_OnSelectedActorChanged(object sender, EventArgs empty)
        {
            UpdateVisual();
        }

        private void UpdateVisual()
        {
            if (ActionSystem.Actor == Actor)
            {
                SelectionAsset.Get<ModelComponent>().Materials.Remove(0);
                SelectionAsset.Get<ModelComponent>().Materials.Add(0, Material);

            }
            else
            {
                SelectionAsset.Get<ModelComponent>().Materials.Remove(0);
                SelectionAsset.Get<ModelComponent>().Materials.Add(0, BlankMaterial);
            }
        }
    }
}
