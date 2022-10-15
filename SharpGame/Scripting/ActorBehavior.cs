using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpGame.Objects;

namespace SharpGame.Scripting
{
    public abstract class ActorBehavior
    {
        private int ActorId { get; set; }
        private Scene Scene { get; set; }

        internal void SetContext(int actorId, Scene scene)
        {
            ActorId = actorId;
            Scene = scene;
        }

        protected ref T GetComponent<T>() where T : struct
        {
            return ref Scene.ActorRegistry.GetComponent<T>(ActorId);
        }
        public abstract void OnAwake();
        public abstract void OnUpdate();
        public abstract void OnSleep();
    }
}
