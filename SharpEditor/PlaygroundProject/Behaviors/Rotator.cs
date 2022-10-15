using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using SharpGame.Input;

using SharpGame.Objects.Components;
using SharpGame.Scripting;

namespace PlaygroundProject.Behaviors
{
    public class Rotator : ActorBehavior
    {
        public override void OnAwake()
        {
        }

        public override void OnUpdate()
        {
            ref TransformComponent transform = ref this.GetComponent<TransformComponent>();
            if (InputSystem.GetKey(KeyCode.W))
            {
                transform.Rotation = Vector3.Add(transform.Rotation, Vector3.UnitZ);
            }
            if (InputSystem.GetKey(KeyCode.S))
            {
                transform.Rotation = Vector3.Add(transform.Rotation, -Vector3.UnitZ);
            }
            if (InputSystem.GetKey(KeyCode.A))
            {
                transform.Rotation = Vector3.Add(transform.Rotation, Vector3.UnitX);
            }
            if (InputSystem.GetKey(KeyCode.D))
            {
                transform.Rotation = Vector3.Add(transform.Rotation, -Vector3.UnitX);
            }
        }

        public override void OnSleep()
        {
        }
    }
}
