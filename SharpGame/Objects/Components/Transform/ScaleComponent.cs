using OpenTK;
using OpenTK.Mathematics;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpGame.Objects.Components
{
    public class ScaleComponent : Component
    {
        public float X { get; private set; }
        public float Y { get; private set; }
        public float Z { get; private set; }

        public ScaleComponent(float x, float y, float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public void Set(Vector3 newPosition)
        {
            Set(newPosition.X, newPosition.Y, newPosition.Z);
        }

        public void Set(float x, float y, float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public void Scale(Vector3 delta)
        {
            this.X += delta.X;
            this.Y += delta.Y;
            this.Z += delta.Z;
        }

        public void Scale(float x, float y, float z)
        {
            this.X += x;
            this.Y += y;
            this.Z += z;
        }
    }
}
