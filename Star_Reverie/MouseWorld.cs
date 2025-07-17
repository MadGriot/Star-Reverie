using Star_Reverie.Globals;
using StarReverieCore.Grid;
using Stride.Animations;
using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Graphics;
using Stride.Input;
using Stride.Physics;
using System;

namespace Star_Reverie
{
    public class MouseWorld : SyncScript
    {


        public CameraComponent Camera;
        internal Simulation simulation;
        public CollisionFilterGroupFlags CollideWith;
        public bool CollideWithTriggers;


        //Debugging
        private float debugTimer = 0f;
        private string debugMessage = "";
        public override void Start()
        {
            simulation = this.GetSimulation();
        }

        public override void Update()
        {
            float debugDeltaTime = (float)Game.UpdateTime.Elapsed.TotalSeconds;
            if (debugTimer > 0f)
            {
                DebugText.Print(debugMessage, new Int2(200, 700), new Color4(Color.Beige));
                debugTimer -= debugDeltaTime;
            }
        }

        public Vector3 GetPosition()
        {
            Texture backbuffer = GraphicsDevice.Presenter.BackBuffer;
            Viewport viewport = new Viewport(0, 0, backbuffer.Width, backbuffer.Height);

            Vector3 nearPosition = viewport.Unproject(new Vector3(Input.AbsoluteMousePosition, 0),
                Camera.ProjectionMatrix, Camera.ViewMatrix, Matrix.Identity);

            Vector3 farPosition = viewport.Unproject(new Vector3(Input.AbsoluteMousePosition, 1.0f),
                Camera.ProjectionMatrix, Camera.ViewMatrix, Matrix.Identity);

            HitResult hitResult = simulation.Raycast(nearPosition, farPosition);
            debugMessage = $"Hit Result: {hitResult.Point}";

            debugTimer = 10f;
            hitResult.Point.Y = Math.Clamp(hitResult.Point.Y, 0.0f, 5.0f);
            return hitResult.Point;
        }
    }
}
