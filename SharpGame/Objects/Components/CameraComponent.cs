using OpenTK;

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
