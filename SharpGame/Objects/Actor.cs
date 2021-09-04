using OpenTK;
using OpenTK.Graphics.OpenGL4;

using SharpGame.Graphics.Meshes;
using SharpGame.Objects.Components;
using SharpGame.Util;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace SharpGame.Objects
{
    public class Actor
    {
        public Scene RootScene { get; set; }

        private readonly List<Component> components = new List<Component>((int)SharedConstants.MaxComponents);
        public PositionComponent PositionComponent { get; private set; }
        public RotationComponent RotationComponent { get; private set; }
        public ScaleComponent ScaleComponent { get; private set; }

        public Actor()
        {
            this.PositionComponent = new PositionComponent(0, 0, 0);
            this.RotationComponent = new RotationComponent(0, 0, 0);
            this.ScaleComponent = new ScaleComponent(1, 1, 1);
        }

        /// <summary>
        /// Retuns a component of type T, if present. Returns null if such component is not present.
        /// </summary>
        /// <typeparam name="T">Type of component to search for</typeparam>
        /// <returns>An instance of the component</returns>
        public T GetComponent<T>() where T : Component
        {
            return (T)components.Find((c) => c is T);
        }
        
        /// <summary>
        /// Adds an instance of a component to the actor and returns that instance
        /// </summary>
        /// <typeparam name="T">Type of component to add</typeparam>
        /// <param name="component">Instance of component to add</param>
        /// <returns>An instance of the added component</returns>
        public T AddComponent<T>(T component) where T : Component
        {
            components.Add(component);
            component.Actor = this;
            return component;
        }

        /// <summary>
        /// Returns true if an actor has the component T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>True if the actor has the component specified</returns>
        public bool HasComponent<T>() where T : Component
        {
            return components.Any((c) => c is T);
        }

        /// <summary>
        /// Removes a component of type T
        /// </summary>
        /// <typeparam name="T">Type of the component</typeparam>
        /// <param name="component">Instace of the component to remove</param>
        public void RemoveComponent<T>(T component) where T : Component
        {
            components.Remove(component);
            component.OnShutdown();
        }
        public virtual void OnAwake()
        {
            for (int i = 0; i < components.Count; i++)
            {
                components[i].OnAwake();
            }
        }

        public virtual void OnUpdate(float deltaTime)
        {
            for (int i = 0; i < components.Count; i++)
            {
                components[i].OnUpdate(deltaTime);
            }
        }

        public virtual void OnStart()
        {
        }

        public virtual void OnShutdown()
        {
            for (int i = 0; i < components.Count; i++)
            {
                components[i].OnShutdown();
            }
        }
    }
}
