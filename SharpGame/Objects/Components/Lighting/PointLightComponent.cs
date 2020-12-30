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

        public PointLightComponent(Vector3 color)
        {
            this.Color = color;
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
