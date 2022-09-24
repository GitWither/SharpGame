using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;

namespace SharpGame.Scripting
{
    internal class BehaviorManager
    {
        public void Initialize()
        {
            AssemblyLoadContext behaviorAssemblyContext = new AssemblyLoadContext("BehaviorAssemblyContext", true);
            Assembly behaviorAssembly = behaviorAssemblyContext.LoadFromAssemblyPath(Path.GetFullPath("PlaygroundProject.dll"));

            Type testType = behaviorAssembly.GetType("PlaygroundProject.Behaviors.TestBehavior");
            MethodInfo testMethod = testType.GetMethod("Test");
            testMethod.Invoke(Activator.CreateInstance(testType), null);
        }
    }
}
