﻿using OpenTK.Mathematics;

using SharpGame.Events;
using SharpGame.Graphics;
using SharpGame.Objects.Components;
using SharpGame.Physics;
using SharpGame.Util;


using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SharpActors;
using SharpGame.Input;
using System.Numerics;
using SharpGame.Scripting;

namespace SharpGame.Objects
{
    public class Scene
    {
        private ActorRegistry m_ActorRegistry;

        internal ActorRegistry ActorRegistry => m_ActorRegistry;

        public int ActorCount => m_ActorRegistry.Count;


        public bool Running { get; set; }

        private HashSet<int> m_Actors = new HashSet<int>();


        private RenderSystem m_RenderSystem;
        private BehaviorSystem m_BehaviorSystem;


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
            this.m_ActorRegistry.RegisterComponent<BehaviorComponent>();


            m_RenderSystem = m_ActorRegistry.RegisterSystem<RenderSystem, TransformComponent, MeshComponent>();
            m_BehaviorSystem = m_ActorRegistry.RegisterSystem<BehaviorSystem, BehaviorComponent>();
        }

        public void CopyContentFrom(Scene scene)
        {
            void CopyComponent<T>(int actor) where T : struct
            {
                if (scene.m_ActorRegistry.HasComponent<T>(actor))
                {
                    m_ActorRegistry.AddComponent(actor, scene.m_ActorRegistry.GetComponent<T>(actor));
                }
            }

            foreach (int actor in scene.EnumerateActors())
            {
                m_ActorRegistry.CreateActor();

                CopyComponent<MeshComponent>(actor);
                CopyComponent<TransformComponent>(actor);
                CopyComponent<GuiTextComponent>(actor);
                CopyComponent<ParticleEmitterComponent>(actor);
                CopyComponent<CameraComponent>(actor);
                CopyComponent<PointLightComponent>(actor);
                CopyComponent<PlayerControlledComponent>(actor);
                CopyComponent<NameComponent>(actor);
                CopyComponent<BehaviorComponent>(actor);

                m_Actors.Add(actor);
            }
        }


        public void OnAwake()
        {
            SharpGameWindow.Instance.BehaviorManager.OnStart(this);

            ActorView behaviorActors = ActorRegistry.CreateView<BehaviorComponent>();
            foreach (int actor in behaviorActors)
            {
                ref BehaviorComponent behaviorComponent = ref ActorRegistry.GetComponent<BehaviorComponent>(actor);
                SharpGameWindow.Instance.BehaviorManager.OnActorCreate(actor, behaviorComponent.BehaviorClass);
            }

            this.Running = true;
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
            SharpGameWindow.Instance.Renderer.Begin(camera);

            m_RenderSystem.OnRender(m_ActorRegistry, SharpGameWindow.Instance.Renderer);

            SharpGameWindow.Instance.Renderer.End();
        }

        public IEnumerable<int> EnumerateActors()
        {
            foreach (int actor in m_Actors)
            {
                yield return actor;
            }
        }

        public void OnUpdate(float deltaTime)
        {
            if (this.Running)
                this.m_BehaviorSystem.OnUpdate();
        }

        public void OnSleep()
        {
            this.Running = false;

            SharpGameWindow.Instance.BehaviorManager.OnStop();
        }
    }
}
