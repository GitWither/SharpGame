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
    public struct CameraComponent
    {
        public float FieldOfView { get; private set; }
        public float NearClipPlane { get; private set; }
        public float FarClipPlane { get; private set; }
        public float AspectRatio { get; private set; }
        public CameraComponent(float fieldOfView, float aspectRatio, float nearClipPlane, float farClipPlane)
        {
            this.FieldOfView = MathHelper.DegreesToRadians(fieldOfView);
            this.NearClipPlane = nearClipPlane;
            this.FarClipPlane = farClipPlane;
            this.AspectRatio = aspectRatio;
        }
    }
}
