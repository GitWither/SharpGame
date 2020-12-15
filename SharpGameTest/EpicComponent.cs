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
        public override void OnAwake()
        {
            this.Actor.PositionComponent.Set(0, 15, 0);
        }
        public override void OnUpdate(float deltaTime)
        {
            this.Actor.RotationComponent.Rotate(0, 2 * deltaTime, 0);
        }
    }
}
