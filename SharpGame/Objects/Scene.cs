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
        private readonly Texture missing;

        private Renderer renderer;


        public Scene()
        {
            missing = new Texture("missing");
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

        public void BindTexture()
        {
            missing.Bind(TextureUnit.Texture0);
        }

        public void Render()
        {
            renderer.Render();
        }

        public void OnUpdate()
        {
            foreach (Actor obj in actors)
            {
                //FIXME: Make this a for loop later
                obj?.OnUpdate();
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

        public void Dispose()
        {
        }
    }
}
