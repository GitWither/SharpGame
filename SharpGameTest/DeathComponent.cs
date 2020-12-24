using SharpGame.Objects.Components;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpGameTest
{
    class DeathComponent : Component
    {
        float timeAlive = 0;

        public override void OnUpdate(float deltaTime)
        {
            timeAlive++;

            if (timeAlive >= 500)
            {
                this.Actor.RootScene.RemoveActor(this.Actor);
            }
        }
    }
}
