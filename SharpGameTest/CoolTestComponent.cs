using SharpGame.Objects.Components;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpGameTest
{
    class CoolTestComponent : Component
    {
        public override void OnUpdate(float deltaTime)
        {
            GuiTextComponent text = this.Actor.GetComponent<GuiTextComponent>();

            text.Text = deltaTime.ToString();

            this.Actor.RotationComponent.Rotate(0, 1, 0);
        }
    }
}
