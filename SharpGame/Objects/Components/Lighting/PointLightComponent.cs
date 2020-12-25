using OpenTK;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpGame.Objects.Components
{
    class PointLightComponent : Component
    {
        public Vector3 Color { get; private set; }

        public PointLightComponent(Vector3 color)
        {
            this.Color = color;
        }

    }
}
