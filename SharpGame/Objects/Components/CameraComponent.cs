using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpGame.Objects.Components
{
    public class CameraComponent : Component
    {
        public float FieldOfView { get; private set; }
        public float NearClipPlane { get; private set; }
        public float FarClipPlane { get; private set; }
        public Matrix4 View { get; private set; }
        public Matrix4 Projection { get; private set; }
        public CameraComponent(float fieldOfView, float nearClipPlane, float farClipPlane)
        {
            this.FieldOfView = MathHelper.DegreesToRadians(fieldOfView);
            this.NearClipPlane = nearClipPlane;
            this.FarClipPlane = farClipPlane;
            this.Projection = Matrix4.CreatePerspectiveFieldOfView(this.FieldOfView, 16 / 9f, this.NearClipPlane, this.FarClipPlane);
            this.View = Matrix4.Identity;
        }

        public override void OnAwake()
        {
            this.Actor.RootScene.Camera = this;
        }

        public override void OnUpdate()
        {
            KeyboardState keyboardState = Keyboard.GetState();
            Vector3 movementDelta = Vector3.Zero;


            if (keyboardState.IsKeyDown(Key.D))
            {
                movementDelta.X += 1;
            }
            if (keyboardState.IsKeyDown(Key.A))
            {
                movementDelta.X -= 1;
            }
            if (keyboardState.IsKeyDown(Key.Space))
            {
                movementDelta.Y += 1;
            }
            if (keyboardState.IsKeyDown(Key.ShiftLeft))
            {
                movementDelta.Y -= 1;
            }
            if (keyboardState.IsKeyDown(Key.S))
            {
                movementDelta.Z += 1;
            }
            if (keyboardState.IsKeyDown(Key.W))
            {
                movementDelta.Z -= 1;
            }
            if (keyboardState.IsKeyDown(Key.Z))
            {
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            }
            if (keyboardState.IsKeyUp(Key.Z))
            {
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            }

            if (movementDelta.LengthSquared > 0.0001f)
            {
                this.View *= Matrix4.CreateTranslation(-movementDelta.Normalized() * 0.1f);
            }
        }

        public override void OnShutdown()
        {
            this.Actor.RootScene.Camera = null;
        }

        public void SetFieldOfView(float fieldOfView)
        {
            this.FieldOfView = MathHelper.DegreesToRadians(fieldOfView);
            this.Projection = Matrix4.CreatePerspectiveFieldOfView(this.FieldOfView, 16 / 9f, this.NearClipPlane, this.FarClipPlane);
        }
    }
}
