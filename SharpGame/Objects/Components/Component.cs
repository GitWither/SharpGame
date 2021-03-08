using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpGame.Objects.Components
{
    public abstract class Component
    {
        public Actor Actor { get; internal set; }
        public virtual void OnAwake()
        {

        }

        public virtual void OnUpdate(float deltaTime)
        {

        }

        public virtual void OnShutdown()
        {

        }
    }
}
