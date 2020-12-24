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
        public override void OnUpdate(float deltaTime)
        {
            hey += 0.1f;
            this.Actor.PositionComponent.Set((float)Math.Cos(hey), 0, 0);
        }
    }
}
