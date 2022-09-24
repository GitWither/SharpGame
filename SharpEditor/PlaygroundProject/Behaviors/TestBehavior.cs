using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpGame.Scripting;

namespace PlaygroundProject.Behaviors
{
    public class TestBehavior : ActorBehavior
    {
        public void Test()
        {
            Console.WriteLine("hello from new dll!");
        }
    }
}
