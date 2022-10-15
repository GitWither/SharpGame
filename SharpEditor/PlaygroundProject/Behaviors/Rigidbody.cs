using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using SharpGame.Objects.Components;
using SharpGame.Scripting;

namespace PlaygroundProject.Behaviors
{
    public class Rigidbody : ActorBehavior
    {
        private float m_Velocity = 0.0f;
        public override void OnAwake()
        {
        }

        public override void OnUpdate()
        {

            ref TransformComponent transform = ref this.GetComponent<TransformComponent>();

            if (transform.Position.Y > 0)
            {
                m_Velocity += 0.981f;
            }
            else
            {
                m_Velocity = 0;
            }

            transform.Position =
                new Vector3(transform.Position.X, transform.Position.Y - m_Velocity, transform.Position.Z);
        }

        public override void OnSleep()
        {
        }
    }
}
