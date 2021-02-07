using SharpGame.Graphics;
using SharpGame.Objects.Components;
using SharpGame.Physics;
using SharpGame.Util;

using System;
using System.Collections.Generic;

namespace SharpGame.Objects
{
    public class Scene : IDisposable
    {
        public CameraComponent Camera { get; set; }
        internal PointLightComponent[] PointLights { get; set; }

        public int ActorCount
        {
            get => actors.Count;
        }

        private bool isRunning = false;

        private readonly List<Actor> actors;

        private RenderSystem renderSystem;
        private PhysicsSystem physicsSystem;


        public Scene()
        {
            actors = new List<Actor>(SharedConstants.MaxActors);

            PointLights = new PointLightComponent[SharedConstants.MaxLights];
        }

        ~Scene()
        {
            actors.Clear();
        }

        internal void OnAwake()
        {
            if (physicsSystem == null && renderSystem == null)
            {
                Logger.Error("Render and physics systems have not been initialized. A register is required before using them!");
                return;
            }
            this.isRunning = true;
        }

        /// <summary>
        /// Registers a rendering system.
        /// </summary>
        /// <param name="renderSystem">An instance of a render system</param>
        public void RegisterRenderSystem(RenderSystem renderSystem)
        {
            if (!this.isRunning)
            {
                this.renderSystem = renderSystem;
            }
            else
            {
                Logger.Error("Cannot register system while scene is running");
            }
        }

        /// <summary>
        /// Registers a physics system.
        /// </summary>
        /// <param name="physicsSystem">An instance of a physics system</param>
        public void RegisterPhysicsSystem(PhysicsSystem physicsSystem)
        {
            if (!this.isRunning)
            {
                this.physicsSystem = physicsSystem;
            }
            else
            {
                Logger.Error("Cannot register system while scene is running");
            }
        }

        /// <summary>
        /// Adds an actor to the scene
        /// </summary>
        /// <param name="actor">An instance of the actor to add to the scene</param>
        public void AddActor(Actor actor)
        {
            actor.RootScene = this;
            actor.OnAwake();
            actors.Add(actor);
            actor.OnStart();
            renderSystem.AddActor(actor);
        }

        public void SetSkyboxMaterial(SkyboxMaterial skyboxMaterial)
        {
            this.renderSystem.SetSkyboxMaterial(skyboxMaterial);
        }

        internal void Render()
        {
            renderSystem.Render();
        }

        internal void OnUpdate(float deltaTime)
        {
            physicsSystem.OnUpdate(deltaTime);
            for (int i = 0; i < actors.Count; i++)
            {
                actors[i].OnUpdate(deltaTime);
            }
        }

        internal void OnShutdown()
        {
            physicsSystem.OnShutdown();
            foreach (Actor actor in actors)
            {
                actor.OnShutdown();
            }
            this.isRunning = false;
        }

        /// <summary>
        /// Retuns an array of actors that have the component specified
        /// </summary>
        /// <typeparam name="T">Component to search for</typeparam>
        /// <returns></returns>
        public Actor[] GetActorsByComponent<T>() where T: Component
        {
            return actors.FindAll(actor => actor.HasComponent<T>()).ToArray();
        }

        /// <summary>
        /// Removes an actor from the scene
        /// </summary>
        /// <param name="actor">An instance of the actor to remove from the scene</param>
        public void RemoveActor(Actor actor)
        {
            actors.Remove(actor);
        }

        public void Dispose()
        {
            actors.Clear();
        }
    }
}
