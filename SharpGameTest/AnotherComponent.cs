using OpenTK;
using OpenTK.Input;

using SharpGame;
using SharpGame.Objects.Components;
using SharpGame.Input;
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
            Vector2 mouseDelta = InputSystem.GetMouseAxis() * 0.05f;
            this.Actor.RotationComponent.Rotate(-mouseDelta.Y, mouseDelta.X, 0);

            if (this.Actor.RotationComponent.Pitch >= 89)
            {
                this.Actor.RotationComponent.Set(89, this.Actor.RotationComponent.Yaw, this.Actor.RotationComponent.Roll);
            }
            else if (this.Actor.RotationComponent.Pitch <= -89)
            {
                this.Actor.RotationComponent.Set(-89, this.Actor.RotationComponent.Yaw, this.Actor.RotationComponent.Roll);
            }
        }
    }
}
