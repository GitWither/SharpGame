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
    public readonly struct Actor
    {
        public static readonly Actor Null = new Actor(-1, null);


        private readonly Scene m_RootScene;
        private readonly int m_Id;


        public Actor(int id, Scene scene)
        {
            this.m_Id = id;
            this.m_RootScene = scene;
        }

        /// <summary>
        /// Retuns a component of type T, if present. Returns null if such component is not present.
        /// </summary>
        /// <typeparam name="T">Type of component to search for</typeparam>
        /// <returns>An instance of the component</returns>
        public ref T GetComponent<T>() where T : struct
        {
            return ref m_RootScene.ActorRegistry.GetComponent<T>(m_Id);
        }
        
        /// <summary>
        /// Adds an instance of a component to the actor and returns that instance
        /// </summary>
        /// <typeparam name="T">Type of component to add</typeparam>
        /// <param name="component">Instance of component to add</param>
        /// <returns>An instance of the added component</returns>
        public T AddComponent<T>(T component) where T : struct
        {
            return m_RootScene.ActorRegistry.AddComponent<T>(m_Id, component);
        }

        /// <summary>
        /// Returns true if an actor has the component T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>True if the actor has the component specified</returns>
        public bool HasComponent<T>() where T : struct
        {
            return m_RootScene.ActorRegistry.HasComponent<T>(m_Id);
        }

        /// <summary>
        /// Removes a component of type T
        /// </summary>
        /// <typeparam name="T">Type of the component</typeparam>
        /// <param name="component">Instance of the component to remove</param>
        public void RemoveComponent<T>() where T : struct
        {
            m_RootScene.ActorRegistry.RemoveComponent<T>(m_Id);
        }

        public static implicit operator int(Actor actor) => actor.m_Id;

        public static bool operator ==(Actor actor1, Actor actor2)
        {
            return actor1.m_Id == actor2.m_Id;
        }

        public static bool operator !=(Actor a, Actor b)
        {
            return a.m_Id != b.m_Id;
        }

        public override bool Equals(object obj)
        {
            if (obj is Actor actor) return m_Id == actor.m_Id;

            return false;
        }

        public override int GetHashCode()
        {
            return m_Id;
        }
    }
}
