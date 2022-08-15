using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;

namespace SharpGame.Graphics
{
    public class Camera
    {
        public Matrix4 Projection { get; protected set; }
        public Matrix4 View { get; protected set; }
        public float Fov { get; private set; }
        public float AspectRatio { get; protected set; }
        public float Near { get; private set; }
        public float Far { get; private set; }

        public Camera(float fov, float aspectRatio, float near, float far)
        {
            this.Fov = fov;
            this.AspectRatio = aspectRatio;
            this.Near = near;
            this.Far = far;

            this.Projection =
                Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(fov), aspectRatio, near, far);
            //this.View = Matrix4.LookAt(Vector3.Zero, Vector3.One, Vector3.UnitY);
        }
    }
}
