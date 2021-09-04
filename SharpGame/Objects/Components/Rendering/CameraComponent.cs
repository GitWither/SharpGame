using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using OpenTK.Mathematics;

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
        public float AspectRatio { get; private set; }
        public Matrix4 View { get; private set; }
        public Matrix4 Projection { get; private set; }
        public CameraComponent(float fieldOfView, float aspectRatio, float nearClipPlane, float farClipPlane)
        {
            this.FieldOfView = MathHelper.DegreesToRadians(fieldOfView);
            this.NearClipPlane = nearClipPlane;
            this.FarClipPlane = farClipPlane;
            this.AspectRatio = aspectRatio;
            this.Projection = Matrix4.CreatePerspectiveFieldOfView(this.FieldOfView, AspectRatio, this.NearClipPlane, this.FarClipPlane);
        }

        public override void OnAwake()
        {
            Logger.Info("test");
            this.Actor.RootScene.Camera = this;

            this.Actor.RootScene.ResolutionChanged += this.ResolutionChanged;
        }

        private void ResolutionChanged(object sender, Events.ScreenEventArgs e)
        {
            Logger.Info("resizing");
            this.SetAspectRatio(e.Size.X / (float)e.Size.Y);
        }

        public override void OnUpdate(float deltaTime)
        {
            //Logger.Info("rendering");
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

            this.Actor.RootScene.ResolutionChanged -= this.ResolutionChanged;
        }

        public void SetFieldOfView(float fieldOfView)
        {
            this.FieldOfView = MathHelper.DegreesToRadians(fieldOfView);
            this.Projection = Matrix4.CreatePerspectiveFieldOfView(this.FieldOfView, AspectRatio, this.NearClipPlane, this.FarClipPlane);
        }

        public void SetAspectRatio(float ratio)
        {
            this.AspectRatio = ratio;
            this.Projection = Matrix4.CreatePerspectiveFieldOfView(this.FieldOfView, AspectRatio, this.NearClipPlane, this.FarClipPlane);
        }
    }
}
