using Star_Reverie.Globals;
using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Input;
using Stride.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Star_Reverie
{
    public class Actor : SyncScript
    {
        private Vector3 targetPosition;
        private Vector3 currentPosition;
        private CharacterComponent characterComponent;

        public override void Start()
        {
            characterComponent = Entity.Get<CharacterComponent>();
        }

        public override void Update()
        {

            if (CurrentGameState.GameState != GameState.Encounter)
            {
                if (Input.IsKeyPressed(Keys.Q))
                {
                    CurrentGameState.GameState = GameState.Encounter;
                }

                return;
            }
            float stoppingDistance = 0.1f;
            currentPosition = Entity.Transform.Position;
            if (Vector3.Distance(currentPosition, targetPosition) > stoppingDistance)
            {
                Vector3 velocity = Vector3.Normalize(targetPosition - currentPosition);
                float moveSpeed = 4f;
                float deltaTime = (float)Game.UpdateTime.Elapsed.TotalSeconds;
                characterComponent.SetVelocity(velocity * moveSpeed);
            }
            else
            {
                characterComponent.SetVelocity(Vector3.Zero);
            }

            if (Input.IsKeyPressed(Keys.T))
            {
                Move(new Vector3(4, 0, 4));
            }
        }

        private void Move(Vector3 targetPosition)
        {
            this.targetPosition = targetPosition;
        }
    }
}
