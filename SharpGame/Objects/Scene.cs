using OpenTK;
using OpenTK.Graphics.OpenGL4;

using SharpGame.Graphics;
using SharpGame.Graphics.Meshes;
using SharpGame.Objects.Components;
using SharpGame.Physics;
using SharpGame.Util;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpGame.Objects
{
    public class Scene : IDisposable
    {
        public CameraComponent Camera { get; set; }
        public PointLightComponent[] PointLights { get; set; }
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

        public void OnAwake()
        {
            if (physicsSystem == null && renderSystem == null)
            {
                Logger.Error("Render and physics systems have not been initialized. A register is required before using them!");
                return;
            }
            this.isRunning = true;
        }

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

        public void AddActor(Actor actor)
        {
            actor.RootScene = this;
            actor.OnAwake();
            actors.Add(actor);
            actor.OnStart();
            renderSystem.AddActor(actor);
        }

        public void Render()
        {
            renderSystem.Render();
        }

        public void OnUpdate(float deltaTime)
        {
            physicsSystem.OnUpdate(deltaTime);
            for (int i = 0; i < actors.Count; i++)
            {
                actors[i].OnUpdate(deltaTime);
            }
        }

        public void OnShutdown()
        {
            physicsSystem.OnShutdown();
            foreach (Actor actor in actors)
            {
                actor.OnShutdown();
            }
            this.isRunning = false;
        }

        public Actor[] GetActorsByComponent<T>() where T: Component
        {
            return actors.FindAll(actor => actor.HasComponent<T>()).ToArray();
        }

        public int GetActorAmount()
        {
            return actors.Count;
        }

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
