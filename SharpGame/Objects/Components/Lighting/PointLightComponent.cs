using OpenTK;
using OpenTK.Mathematics;

using SharpGame.Util;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpGame.Objects.Components
{
    public struct PointLightComponent
    {
        public Vector3 Color { get; set; }
        public float MaxDistance { get; set; }

        public PointLightComponent(Vector3 color, float maxDistance)
        {
            this.Color = color;
            this.MaxDistance = maxDistance;
        }
    }
}
