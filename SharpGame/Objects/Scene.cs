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
using SharpGame.Objects.Components.Transform;

namespace SharpGame.Objects
{
    public class Scene
    {
        private readonly ActorRegistry m_ActorRegistry;

        internal ActorRegistry ActorRegistry => m_ActorRegistry;

        public int ActorCount => m_ActorRegistry.Count;


        public bool Running { get; set; }


        private RenderSystem m_RenderSystem;
        private Renderer m_Renderer = new Renderer();


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


            m_RenderSystem = m_ActorRegistry.RegisterSystem<RenderSystem, TransformComponent, MeshComponent>();

            m_Renderer.Initialize();
        }

        internal void OnAwake()
        {
        }


        /// <summary>
        /// Adds an actor to the scene
        /// </summary>
        /// <param name="actor">An instance of the actor to add to the scene</param>
        public Actor CreateActor()
        {
            return new Actor(m_ActorRegistry.CreateActor(), this);
        }

        public void RemoveActor(Actor actor)
        {
            m_ActorRegistry.DestroyActor(actor);
        }

        public void OnRender(Camera camera)
        {
            m_Renderer.Begin(camera);

            m_RenderSystem.OnRender(m_ActorRegistry, m_Renderer);

            m_Renderer.End();
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
