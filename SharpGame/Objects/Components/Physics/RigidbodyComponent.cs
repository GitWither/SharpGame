using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpGame.Objects.Components.Physics
{
    class RigidbodyComponent : Component
    {
        public float Mass { get; set; }

        public RigidbodyComponent(float mass)
        {
            this.Mass = mass;
        }
    }
}
