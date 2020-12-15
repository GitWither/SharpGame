using SharpGame.Objects.Components;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpGameTest
{
    class EpicComponent : Component
    {
        float yaw;

        public override void OnAwake()
        {
            this.Actor.PositionComponent.Set(0, 15, 0);
        }
        public override void OnUpdate()
        {
            yaw += 0.1f;
            if (yaw >= 360)
            {
                yaw = 0;
            }
            this.Actor.RotationComponent.Set(0, yaw, 0);
        }
    }
}
