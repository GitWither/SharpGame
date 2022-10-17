using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using SharpGame.Objects;
using SharpGame.Objects.Components;

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

        protected Vector3 Forward
        {
            get
            {
                //TODO: Adjust this to support elevation as well
                ref TransformComponent transform = ref Scene.ActorRegistry.GetComponent<TransformComponent>(ActorId);
                return new Vector3(MathF.Sin(transform.Rotation.Y), 0, MathF.Cos(transform.Rotation.Y));
            }
        }

        protected Vector3 Right => Vector3.Cross(Forward, Vector3.UnitY);

        protected ref T GetComponent<T>() where T : struct
        {
            return ref Scene.ActorRegistry.GetComponent<T>(ActorId);
        }

        //TODO: Placeholder until ref properties are available
        protected ref TransformComponent GetPosition()
        {
            return ref Scene.ActorRegistry.GetComponent<TransformComponent>(ActorId);
        }
        public abstract void OnAwake();
        public abstract void OnUpdate();
        public abstract void OnSleep();
    }
}
