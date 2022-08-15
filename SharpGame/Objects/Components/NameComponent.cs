using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpGame.Objects.Components
{
    public struct NameComponent
    {
        public string Name { get; set; }

        public NameComponent(string name)
        {
            this.Name = name;
        }
    }
}
