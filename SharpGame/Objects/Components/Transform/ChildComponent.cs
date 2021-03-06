using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpGame.Objects.Components
{
    public class ChildComponent : Component
    {
        public Actor Parent { get; set; }

        public ChildComponent(Actor parent)
        {
            this.Parent = parent;
        }

        public override void OnUpdate(float deltaTime)
        {
            
        }
    }
}
