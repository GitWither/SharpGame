using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SharpGame.Objects.Components
{
    public struct BehaviorComponent {
        public Type BehaviorClass { get; set; }

        public BehaviorComponent(Type type)
        {
            this.BehaviorClass = type;
        }
    }
}
