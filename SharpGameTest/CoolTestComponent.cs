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
        float frameTime;
        float fps;
        GuiTextComponent text;
        public override void OnAwake()
        {
            text = this.Actor.GetComponent<GuiTextComponent>();
        }
        public override void OnUpdate(float deltaTime)
        {
            frameTime += deltaTime;
            fps++;
            if (frameTime >= 1)
            {
                frameTime = 0;
                text.Text = "FPS: " + fps.ToString();
                fps = 0;
            }

            //text.Text = deltaTime.ToString();

            //this.Actor.RotationComponent.Rotate(0, 1, 0);
        }
    }
}
