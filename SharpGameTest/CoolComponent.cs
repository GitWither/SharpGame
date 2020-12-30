using SharpGame.Objects.Components;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpGameTest
{
    class CoolComponent : Component
    {
        float hey = 0;
        float dirX = 0;
        float dirZ = 0;

        public CoolComponent(float dirX, float dirZ)
        {
            this.dirX = dirX;
            this.dirZ = dirZ;
        }
        public override void OnUpdate(float deltaTime)
        {
            hey += 0.1f;
            this.Actor.PositionComponent.Set(dirX * (float)Math.Cos(hey / 15) * 25, 15, dirZ * (float)Math.Cos(hey / 15) * 25);
        }
    }
}
