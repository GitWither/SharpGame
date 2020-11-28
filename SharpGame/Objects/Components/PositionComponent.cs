using OpenTK;

using SharpGame.Util;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpGame.Objects.Components
{
    public class PositionComponent : Component
    {
        public float X { get; private set; }
        public float Y { get; private set; }
        public float Z { get; private set; }

        public PositionComponent(float x, float y, float z)
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

        public void Translate(Vector3 delta)
        {
            this.X += delta.X;
            this.Y += delta.Y;
            this.Z += delta.Z;
        }

        public void Translate(float x, float y, float z)
        {
            this.X += x;
            this.Y += y;
            this.Z += z;
        }

        public static implicit operator Vector3(PositionComponent positionComponent)
        {
            return new Vector3(positionComponent.X, positionComponent.Y, positionComponent.Z);
        } 
    }
}
