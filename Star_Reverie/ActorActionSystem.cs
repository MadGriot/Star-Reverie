using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation.Text;

namespace Star_Reverie
{
    public class ActorActionSystem : SyncScript
    {
        // Declared public member fields and properties will show in the game studio
        public Entity Actor { get; set; }
        public MouseWorld MouseWorld { get; set; }

        public override void Start()
        {
            MouseWorld = Actor.Get<MouseWorld>();
        }

        public override void Update()
        {
            if (Input.IsMouseButtonPressed(MouseButton.Left))
            {
               MouseWorld.Move(MouseWorld.GetPosition());
                MouseWorld.animationState = AnimationState.Running;
                MouseWorld.PlayAnimation(MouseWorld.animationState);
            }
        }
    }
}
