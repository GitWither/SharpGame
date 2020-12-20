using OpenTK;
using OpenTK.Input;

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
    class AnotherComponent : Component
    {
        public override void OnUpdate(float deltaTime)
        {
            /*
            MouseState newMouseState = Mouse.GetState();
            if (Input.mouseState.HasValue)
            {
                //return (mouseAxis == MouseAxis.X) ? (newMouseState.Y - mouseState.Value.Y) : (mouseState.Value.X - newMouseState.X);
                Logger.Info(Input.mouseState.Value.X - newMouseState.X);
            }
            Input.mouseState = newMouseState;
            */
            //this.Actor.RotationComponent.Rotate(-Input.GetAxis(MouseAxis.Y) * 0.05f, -Input.GetAxis(MouseAxis.X) * 0.05f, 0);
           // Logger.Info((-Input.GetAxis(MouseAxis.X) * 0.05f) + " " + (-Input.GetAxis(MouseAxis.X) * 0.05f));
        }
    }
}
