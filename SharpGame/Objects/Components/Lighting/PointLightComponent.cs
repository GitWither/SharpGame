using OpenTK;

using SharpGame.Util;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpGame.Objects.Components
{
    public class PointLightComponent : Component
    {
        public Vector3 Color { get; private set; }
        public float Distance { get; private set; }

        public PointLightComponent(Vector3 color, float distance)
        {
            this.Color = color;
            this.Distance = distance;
        }

        public override void OnAwake()
        {
            for (int i = 0; i < SharedConstants.MaxLights; i++)
            {
                if (this.Actor.RootScene.PointLights[i] == null)
                {
                    this.Actor.RootScene.PointLights[i] = this;
                    break;
                }
            }
        }

    }
}
