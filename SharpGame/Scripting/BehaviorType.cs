using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SharpGame.Scripting
{
    internal class BehaviorType
    {
        public Type Class { get; private set; }
        public MethodInfo OnAwakeMethod { get; }
        public MethodInfo OnUpdateMethod { get; }
        public MethodInfo OnSleepMethod { get; }

        public BehaviorType(Type type)
        {
            this.Class = type;
            OnAwakeMethod = this.Class.GetMethod("OnAwake");
            OnUpdateMethod = this.Class.GetMethod("OnUpdate");
            OnSleepMethod = this.Class.GetMethod("OnSleep");
        }

        public ActorBehavior CreateInstance()
        {
            return (ActorBehavior)Activator.CreateInstance(Class);
        }

        public void InvokeOnAwake(ActorBehavior instance)
        {
            OnAwakeMethod.Invoke(instance, null);
        }

        public void InvokeOnSleep(ActorBehavior instance)
        {
            OnSleepMethod.Invoke(instance, null);
        }

        public void InvokeOnUpdate(ActorBehavior instance)
        {
            OnUpdateMethod.Invoke(instance, null);
        }
    }
}
