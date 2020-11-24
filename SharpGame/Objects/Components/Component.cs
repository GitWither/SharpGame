using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpGame.Objects.Components
{
    public abstract class Component
    {
        public Actor Actor { get; set; }
        public virtual void OnAwake()
        {

        }

        public virtual void OnUpdate()
        {

        }

        public virtual void OnShutdown()
        {

        }
    }
}
