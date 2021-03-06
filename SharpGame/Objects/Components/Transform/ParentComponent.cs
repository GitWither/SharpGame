using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpGame.Objects.Components.Transform
{
    public class ParentComponent : Component
    {
        public List<Actor> Children { get; set; }

        public ParentComponent(List<Actor> children)
        {
            this.Children = children;
        }

        public override void OnUpdate(float deltaTime)
        {
            for (int i = 0; i < Children.Count; i++)
            {

            }
        }
    }
}
