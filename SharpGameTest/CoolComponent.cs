using SharpGame.Objects.Components;
using SharpGame.Util;

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
            hey += 1f * deltaTime;
            //Logger.Info(hey * deltaTime);
            this.Actor.PositionComponent.Set(dirX * (float)Math.Sin(hey) * 15, 5, dirZ * (float)Math.Cos(hey) * 15);
        }
    }
}
