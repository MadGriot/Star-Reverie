using System;
using Stride.Engine;

namespace Star_Reverie
{
    public class EnemyAI : SyncScript
    {
        public ActorActionSystem ActorActionSystem;
        private float timer;

        public override void Start()
        {
            ActorActionSystem.OnSelectedActorChanged += ActorActionSystem_OnSelectedActorChanged;
        }


        public override void Update()
        {
            if (ActorActionSystem.isPlayerTurn)
                return;

            timer -= (float)Game.UpdateTime.Elapsed.TotalSeconds;
            if (timer <= 0f)
            {
                ActorActionSystem.TurnEnded();
            }
        }

        private void ActorActionSystem_OnSelectedActorChanged(object sender, EventArgs e)
        {
            timer = 2f;
        }
    }
}
