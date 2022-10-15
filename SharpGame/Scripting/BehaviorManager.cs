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
        private AssemblyLoadContext m_AssemblyLoadContext;
        private Assembly m_Assembly;

        private readonly HashSet<BehaviorType> m_BehaviorTypes = new();
        private readonly Dictionary<int, ActorBehavior> m_ActorIdToBehavior = new();
        private readonly Dictionary<Type, BehaviorType> m_TypeToBehavior = new();

        private Scene m_ContextScene;

        public int BehaviorInstances => m_ActorIdToBehavior.Count;

        public void Initialize()
        {
            m_AssemblyLoadContext = new AssemblyLoadContext("BehaviorAssemblyContext", true);
            m_AssemblyLoadContext.Unloading += AssemblyContextUnloaded;
            m_Assembly = LoadAssembly();

            Type[] availableTypes = m_Assembly.GetTypes();

            foreach (Type type in availableTypes)
            {
                if (!type.IsAssignableTo(typeof(ActorBehavior))) continue;

                BehaviorType behaviorType = new BehaviorType(type);
                m_TypeToBehavior.Add(type, behaviorType);
                m_BehaviorTypes.Add(behaviorType);

                FieldInfo[] fields = type.GetFields();
                foreach (FieldInfo field in fields)
                {
                    Logger.Info("Found field: " + field.Name);
                }
            }

            Logger.Info("Loaded classes: " + m_BehaviorTypes.Count);

        }

        private void AssemblyContextUnloaded(AssemblyLoadContext obj)
        {
            m_Assembly = LoadAssembly();
        }

        public IEnumerable<Type> EnumerateBehaviorClasses()
        {
            foreach (BehaviorType type in m_BehaviorTypes)
            {
                yield return type.Class;
            }
        }

        public Type GetTypeFromName(string name)
        {
            return m_Assembly.GetType(name);
        }

        public void OnStart(Scene scene)
        {
            m_ContextScene = scene;
        }

        public void OnStop()
        {
            foreach (ActorBehavior behavior in m_ActorIdToBehavior.Values)
            {
                behavior.OnSleep();
            }
            m_ActorIdToBehavior.Clear();
        }

        private Assembly LoadAssembly()
        {
            using FileStream fs = File.OpenRead("PlaygroundProject.dll");
            return m_AssemblyLoadContext.LoadFromStream(fs);
        }

        public void ReloadAssembly()
        {
            m_AssemblyLoadContext.Unload();
        }

        public void OnUpdate(int actor)
        {
            m_ActorIdToBehavior[actor].OnUpdate();
        }

        public void OnActorCreate(int actor, Type type)
        {
            BehaviorType behaviorType = m_TypeToBehavior[type];
            ActorBehavior behavior = behaviorType.CreateInstance();

            behavior.SetContext(actor, m_ContextScene);

            m_ActorIdToBehavior.Add(actor, behavior);

            behavior.OnAwake();
        }
    }
}
