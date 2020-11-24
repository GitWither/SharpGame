using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpGame.Objects.Components
{
    class PlayerControlledComponent : Component
    {
        public override void OnUpdate()
        {
            base.OnUpdate();
            if (Input.GetKeyDown(Util.KeyCode.W))
            {
                this.Actor.PositionComponent.Translate(0.0f, 0.0f, 0.1f);
            }
            else if (Input.GetKeyDown(Util.KeyCode.A))
            {
                this.Actor.PositionComponent.Translate(-0.1f, 0.0f, 0);
            }
            else if (Input.GetKeyDown(Util.KeyCode.S))
            {
                this.Actor.PositionComponent.Translate(0.0f, 0.0f, -0.1f);
            }
            else if (Input.GetKeyDown(Util.KeyCode.D))
            {
                this.Actor.PositionComponent.Translate(0.1f, 0.0f, 0.0f);
            }
        }
    }
}
