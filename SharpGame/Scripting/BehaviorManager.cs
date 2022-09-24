using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.ES20;
using SharpGame.Objects;
using SharpGame.Util;

namespace SharpGame.Scripting
{
    public class BehaviorManager
    {
        private readonly HashSet<BehaviorType> m_BehaviorTypes = new();
        private readonly Dictionary<int, ActorBehavior> m_ActorIdToBehavior = new();

        public void Initialize()
        {
            AssemblyLoadContext behaviorAssemblyContext = new AssemblyLoadContext("BehaviorAssemblyContext", true);
            Assembly behaviorAssembly = behaviorAssemblyContext.LoadFromAssemblyPath(Path.GetFullPath("PlaygroundProject.dll"));

            Type[] availableTypes = behaviorAssembly.GetTypes();

            foreach (Type type in availableTypes)
            {
                if (!type.IsAssignableTo(typeof(ActorBehavior))) continue;

                m_BehaviorTypes.Add(new BehaviorType(type));

                FieldInfo[] fields = type.GetFields();
                foreach (FieldInfo field in fields)
                {
                    Logger.Info("Found field: " + field.Name);
                }
            }

            Logger.Info("Loaded classes: " + m_BehaviorTypes.Count);

        }

        public IEnumerable<Type> EnumerateBehaviorClasses()
        {
            foreach (BehaviorType type in m_BehaviorTypes)
            {
                yield return type.Class;
            }
        }

        public void OnStart(Scene scene)
        {
            Logger.Info("Loaded classes: " + m_BehaviorTypes.Count);

            foreach (BehaviorType type in m_BehaviorTypes)
            {
                ActorBehavior behavior = type.CreateInstance();
                type.OnAwakeMethod.Invoke(behavior, null);
            }
        }

        public void OnStop()
        {

        }
    }
}
