using OpenTK;
using OpenTK.Graphics.OpenGL4;

using SharpGame.Graphics;
using SharpGame.Graphics.Meshes;
using SharpGame.Objects.Components;
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
        private bool isRunning = false;

        private readonly Actor[] actors = new Actor[SharedConstants.MaxActors];

        private readonly Renderer renderer;


        public Scene()
        {
            renderer = new Renderer();
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
            for (int i = 0; i < SharedConstants.MaxActors; i++)
            {
                if (actors[i] == null)
                {
                    actor.RootScene = this;
                    actor.OnAwake();
                    actors[i] = actor;
                    actor.OnStart();
                    renderer.AddActor(actor);
                    break;
                }
            }
        }

        public void Render()
        {
            renderer.Render();
        }

        public void OnUpdate(float deltaTime)
        {
            for (int i = 0; i < SharedConstants.MaxActors; i++)
            {
                actors[i]?.OnUpdate(deltaTime);
            }
        }

        public void OnShutdown()
        {
            for (int i = 0; i < SharedConstants.MaxActors; i++)
            {
                actors[i]?.OnShutdown();
            }
            this.isRunning = false;
        }

        public Actor[] GetActorsByComponent<T>() where T: Component
        {
            List<Actor> filteredActors = new List<Actor>();
            for (int i = 0; i < SharedConstants.MaxActors; i++)
            {
                if (this.actors[i] != null)
                {
                    if (this.actors[i].HasComponent<T>())
                    {
                        filteredActors.Add(this.actors[i]);
                    }
                }
            }
            return filteredActors.ToArray();
        }

        public void RemoveActor(Actor actor)
        {
            for (int i = 0; i < SharedConstants.MaxActors; i++)
            {
                if (this.actors[i] != null && this.actors[i] == actor)
                {
                    this.actors[i] = null;
                }
            }
        }

        public void Dispose()
        {
        }
    }
}
