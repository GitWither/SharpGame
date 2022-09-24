using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpGame.Scripting;
using SharpGame.Util;

namespace PlaygroundProject.Behaviors
{
    public class TestBehavior : ActorBehavior
    {
        public int CoolField;
        public void Test()
        {
            Logger.Info("this is coming from the loaded DLL!");
        }

        public void OnAwake()
        {
            Logger.Info("Called from onawake!!!");
        }
    }
}
