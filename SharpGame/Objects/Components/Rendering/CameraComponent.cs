using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;

using SharpGame.Util;

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

        public override void OnUpdate(float deltaTime)
        {
            float forwardX = (float)(Math.Sin(MathHelper.DegreesToRadians(this.Actor.RotationComponent.Yaw)) * Math.Cos(MathHelper.DegreesToRadians(this.Actor.RotationComponent.Pitch)));
            float forwardY = (float)Math.Sin(MathHelper.DegreesToRadians(this.Actor.RotationComponent.Pitch));
            float forwardZ = (float)(Math.Cos(MathHelper.DegreesToRadians(this.Actor.RotationComponent.Yaw)) * Math.Cos(MathHelper.DegreesToRadians(this.Actor.RotationComponent.Pitch)));

            this.View = Matrix4.LookAt(
                this.Actor.PositionComponent.X, this.Actor.PositionComponent.Y, this.Actor.PositionComponent.Z,
                this.Actor.PositionComponent.X + forwardX, this.Actor.PositionComponent.Y + forwardY, this.Actor.PositionComponent.Z + forwardZ,
                0, 1, 0
                );
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
