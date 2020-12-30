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

        private readonly Renderer renderer;
        private readonly PhysicsSolver physicsSolver;


        public Scene()
        {
            actors = new List<Actor>(SharedConstants.MaxActors);

            PointLights = new PointLightComponent[SharedConstants.MaxLights];

            renderer = new Renderer();
            physicsSolver = new PhysicsSolver();
        }

        ~Scene()
        {
        }

        public void OnAwake()
        {
            this.isRunning = true;
        }

        public void AddActor(Actor actor)
        {
            actor.RootScene = this;
            actor.OnAwake();
            actors.Add(actor);
            actor.OnStart();
            renderer.AddActor(actor);
        }

        public void Render()
        {
            renderer.Render();
        }

        public void OnUpdate(float deltaTime)
        {
            physicsSolver.OnUpdate(deltaTime);
            for (int i = 0; i < actors.Count; i++)
            {
                actors[i].OnUpdate(deltaTime);
            }
        }

        public void OnShutdown()
        {
            physicsSolver.OnShutdown();
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
