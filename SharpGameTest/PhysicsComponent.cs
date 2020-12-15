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
        float acc = 0;
        public override void OnUpdate()
        {
            if (this.Actor.PositionComponent.Y > 0 && !Input.GetKeyDown(KeyCode.Z)) {
                this.Actor.PositionComponent.Translate(0, acc, 0);
                acc -= 0.0981f;
                Logger.Warn(acc);
            }
            else if (this.Actor.PositionComponent.Y <= 0)
            {
                acc = 0;
            }
        }
    }
}
