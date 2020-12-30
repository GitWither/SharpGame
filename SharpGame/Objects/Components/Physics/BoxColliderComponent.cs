using OpenTK;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpGame.Objects.Components.Physics
{
    class BoxColliderComponent : Component
    {
        public float MinX { get; private set; }
        public float MinY { get; private set; }
        public float MinZ { get; private set; }
        public float MaxX { get; private set; }
        public float MaxY { get; private set; }
        public float MaxZ { get; private set; }

        public BoxColliderComponent(float minX, float minY, float minZ, float maxX, float maxY, float maxZ)
        {
            this.MinX = minX;
            this.MinY = minY;
            this.MinZ = minZ;

            this.MaxX = maxX;
            this.MaxY = maxY;
            this.MaxZ = maxZ;
        }

        public float GetHalfSizeX()
        {
            return (this.MaxX - this.MinX) * 0.5f;
        }

        public float GetHalfSizeY()
        {
            return (this.MaxY - this.MinY) * 0.5f;
        }

        public float GetHalfSizeZ()
        {
            return (this.MaxZ - this.MinZ) * 0.5f;
        }
    }
}
