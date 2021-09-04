using OpenTK.Mathematics;

using SharpGame.Events;
using SharpGame.Graphics;
using SharpGame.Objects.Components;
using SharpGame.Physics;
using SharpGame.Util;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace SharpGame.Objects
{
    public class Scene : IDisposable
    {
        public event EventHandler<ActorEventArgs> ActorAdded;
        public event EventHandler<ActorEventArgs> ActorRemoved;

        public event EventHandler<ScreenEventArgs> ResolutionChanged;

        private readonly ConcurrentQueue<Actor> toBeAddedActorsLogic;
        private readonly ConcurrentQueue<Actor> toBeAddedActorsRender;

        private readonly ConcurrentQueue<Actor> toBeRemovedActorsLogic;
        private readonly ConcurrentQueue<Actor> toBeRemovedActorsRender;

        public CameraComponent Camera { get; set; }
        internal PointLightComponent[] PointLights { get; set; }

        public int ActorCount
        {
            get => actors.Count;
        }

        public bool Running { get; set; }

        private readonly List<Actor> actors;

        internal RenderSystem RenderSystem { get; private set; }
        internal PhysicsSystem PhysicsSystem { get; private set; }


        public Scene()
        {
            Running = false;

            actors = new List<Actor>(SharedConstants.MaxActors);

            toBeAddedActorsRender = new ConcurrentQueue<Actor>();
            toBeAddedActorsLogic = new ConcurrentQueue<Actor>();

            toBeRemovedActorsLogic = new ConcurrentQueue<Actor>();
            toBeRemovedActorsRender = new ConcurrentQueue<Actor>();

            PointLights = new PointLightComponent[SharedConstants.MaxLights];
        }

        ~Scene()
        {
            actors.Clear();
        }

        internal void OnAwake()
        {
            if (PhysicsSystem == null && RenderSystem == null)
            {
                Logger.Error("Render and physics systems have not been initialized. A register is required before using them!");
                return;
            }
            this.Running = true;
        }

        /// <summary>
        /// Registers a rendering system.
        /// </summary>
        /// <param name="renderSystem">An instance of a render system</param>
        public void RegisterRenderSystem(RenderSystem renderSystem)
        {
            if (!this.Running)
            {
                this.RenderSystem = renderSystem;
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
            if (!this.Running)
            {
                this.PhysicsSystem = physicsSystem;
                this.PhysicsSystem.OnAwake();
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
            toBeAddedActorsRender.Enqueue(actor);
            toBeAddedActorsLogic.Enqueue(actor);
            actor.RootScene = this;
            actor.OnAwake();
            actors.Add(actor);
            actor.OnStart();
            //ActorAdded?.Invoke(this, new ActorEventArgs(actor));
        }

        public void SetSkyboxMaterial(SkyboxMaterial skyboxMaterial)
        {
            this.RenderSystem.SetSkyboxMaterial(skyboxMaterial);
        }

        internal void OnRender()
        {
            while (toBeAddedActorsRender.Count > 0)
            {
                if (toBeAddedActorsRender.TryDequeue(out Actor actor))
                {
                    actor.OnAwake();
                    RenderSystem.AddActor(actor);
                }
            }


            RenderSystem.Render();
        }

        internal void OnUpdate(float deltaTime)
        {
            while (toBeAddedActorsLogic.Count > 0)
            {
                if (toBeAddedActorsLogic.TryDequeue(out Actor actor))
                {
                    actor.RootScene = this;
                    actors.Add(actor);
                }
            }

            //PhysicsSystem.OnUpdate(1.0f / 60);
            for (int i = 0; i < actors.Count; i++)
            {
                actors[i].OnUpdate(deltaTime);
            }
        }

        internal void OnShutdown()
        {
            this.Running = false;
            PhysicsSystem.OnShutdown();
            foreach (Actor actor in actors)
            {
                actor.OnShutdown();
            }
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
            toBeRemovedActorsRender.Enqueue(actor);
            toBeRemovedActorsLogic.Enqueue(actor);
            //ActorRemoved?.Invoke(this, new ActorEventArgs(actor));
            //actor.OnShutdown();
            //actors.Remove(actor);
        }

        internal void ScreenResized(Vector2i newSize)
        {
            ResolutionChanged?.Invoke(this, new ScreenEventArgs(newSize));
        }

        public void Dispose()
        {
            actors.Clear();
        }
    }
}
