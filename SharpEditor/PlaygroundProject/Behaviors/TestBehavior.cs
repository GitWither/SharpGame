using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using SharpGame.Input;
using SharpGame.Objects.Components;
using SharpGame.Scripting;
using SharpGame.Util;

namespace PlaygroundProject.Behaviors
{
    public class TestBehavior : ActorBehavior
    {
        public int CoolField;

        public void Test()
        {
            Logger.Info("this is coming from the loaded DLL!");
        }

        public override void OnAwake()
        {
            Logger.Info("Called from onawake (reloaded)!!!");
        }

        public override void OnUpdate()
        {

            ref TransformComponent transform = ref GetPosition();

            if (InputSystem.GetKey(KeyCode.RightArrow))
            {
                transform.Rotation = Vector3.Add(transform.Rotation, Vector3.UnitY * 0.05f);
            }
            if (InputSystem.GetKey(KeyCode.LeftArrow))
            {
                transform.Rotation = Vector3.Add(transform.Rotation, -Vector3.UnitY * 0.05f);
            }

            Vector3 forward = Forward;
            Vector3 right = Vector3.Cross(forward, Vector3.UnitY);


            if (InputSystem.GetKey(KeyCode.W))
            {
                transform.Position = Vector3.Add(transform.Position, -forward);
            }
            if (InputSystem.GetKey(KeyCode.S))
            {
                transform.Position = Vector3.Add(transform.Position, forward);
            }
            if (InputSystem.GetKey(KeyCode.A))
            {
                transform.Position = Vector3.Add(transform.Position, right);
            }
            if (InputSystem.GetKey(KeyCode.D))
            {
                transform.Position = Vector3.Add(transform.Position, -right);
            }

            if (InputSystem.GetKey(KeyCode.Space))
            {
                transform.Position = Vector3.Add(transform.Position, Vector3.UnitY);
            }

            if (InputSystem.GetKey(KeyCode.LeftShift))
            {
                transform.Position = Vector3.Add(transform.Position, -Vector3.UnitY);
            }
        }

        public override void OnSleep()
        {
            Logger.Info("going to sleep");
        }
    }
}
