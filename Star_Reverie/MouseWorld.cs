using Star_Reverie.Globals;
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

        public override void Start()
        {
            simulation = this.GetSimulation();
        }

        public override void Update()
        {

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

            return hitResult.Point;
        }
    }
}
