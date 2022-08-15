using OpenTK.Mathematics;

using SharpGame.Events;
using SharpGame.Graphics;
using SharpGame.Objects.Components;
using SharpGame.Physics;
using SharpGame.Util;


using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using SharpActors;
using SharpGame.Input;

namespace SharpGame.Objects
{
    public class Scene
    {
        private readonly ActorRegistry m_ActorRegistry;

        internal ActorRegistry ActorRegistry => m_ActorRegistry;

        public int ActorCount => m_ActorRegistry.Count;


        public bool Running { get; set; }

        private HashSet<int> m_Actors = new HashSet<int>();


        private RenderSystem m_RenderSystem;
        private Renderer Renderer { get; set; }


        public Scene()
        {
            Running = false;
            this.m_ActorRegistry = new ActorRegistry(SharedConstants.MaxActors);

            this.m_ActorRegistry.RegisterComponent<MeshComponent>();
            this.m_ActorRegistry.RegisterComponent<TransformComponent>();
            this.m_ActorRegistry.RegisterComponent<GuiTextComponent>();
            this.m_ActorRegistry.RegisterComponent<ParticleEmitterComponent>();
            this.m_ActorRegistry.RegisterComponent<CameraComponent>();
            this.m_ActorRegistry.RegisterComponent<PointLightComponent>();
            this.m_ActorRegistry.RegisterComponent<PlayerControlledComponent>();
            this.m_ActorRegistry.RegisterComponent<NameComponent>();


            m_RenderSystem = m_ActorRegistry.RegisterSystem<RenderSystem, TransformComponent, MeshComponent>();

            Renderer = new Renderer();
            Renderer.Initialize();
        }

        internal void OnAwake()
        {
        }


        /// <summary>
        /// Adds an actor to the scene
        /// </summary>
        /// <param name="actor">An instance of the actor to add to the scene</param>
        public Actor CreateActor(string name)
        {
            int id = ActorRegistry.CreateActor();
            ActorRegistry.AddComponent(id, new TransformComponent());
            ActorRegistry.AddComponent(id, new NameComponent(name));
            m_Actors.Add(id);
            return new Actor(id, this);
        }

        public void RemoveActor(Actor actor)
        {
            m_ActorRegistry.DestroyActor(actor);
            m_Actors.Remove(actor);
        }

        public void OnRender(Camera camera)
        {
            Renderer.Begin(camera);

            m_RenderSystem.OnRender(m_ActorRegistry, Renderer);

            Renderer.End();
        }

        public IEnumerable<int> EnumarateActors()
        {
            foreach (int actor in m_Actors)
            {
                yield return actor;
            }
        }

        internal void OnUpdate(float deltaTime)
        {
        }

        internal void OnShutdown()
        {
            this.Running = false;
        }
    }
}
