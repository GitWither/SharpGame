using SharpGame;
using SharpGame.Objects.Components;
using SharpGame.Util;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpGameTest
{
    class PhysicsComponent : Component
    {
        float acc = 55;
        public override void OnUpdate(float deltaTime)
        {
            if (this.Actor.PositionComponent.Y > 0) {
                this.Actor.PositionComponent.Translate(0, acc * deltaTime, 0);
                acc -= 0.981f;
            }
            else if (this.Actor.PositionComponent.Y <= 1.8)
            {
                acc = 0;
            }
        }
    }
}
