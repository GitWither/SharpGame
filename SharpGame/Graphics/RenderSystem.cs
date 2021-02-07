using OpenTK.Audio.OpenAL;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

using SharpGame.Graphics.Meshes;
using SharpGame.Graphics.Vaos;
using SharpGame.Objects;
using SharpGame.Objects.Components;
using SharpGame.Util;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpGame.Graphics
{
    public class RenderSystem : IDisposable
    {
        private readonly List<VertexArrayObject> worldObjects;
        private readonly List<VertexArrayObject> guiObjects;
        private SkybockVertexArrayObject skybockVertexArrayObject;

        public RenderSystem()
        {
            worldObjects = new List<VertexArrayObject>();
            guiObjects = new List<VertexArrayObject>();
        }

        ~RenderSystem()
        {
            this.Dispose();
        }

        public void AddActor(Actor actor)
        {
            MeshRendererComponent meshRendererComponent = actor.GetComponent<MeshRendererComponent>();
            if (meshRendererComponent != null)
            {
                Add(meshRendererComponent);
            }
            else
            {
                Logger.Info("Actor does not have mesh renderer component. Not adding to renderer.");
            }
        }

        public void SetSkyboxMaterial(SkyboxMaterial skyboxMaterial)
        {
            if (skyboxMaterial != null)
            {
                skybockVertexArrayObject = new SkybockVertexArrayObject();
                skybockVertexArrayObject.SetSkyboxMaterial(skyboxMaterial);
                skybockVertexArrayObject.Upload();
            }
            else
            {
                skybockVertexArrayObject = null;
            }
        } 

        private void Add(MeshRendererComponent meshRendererComponent)
        {
            VertexArrayObject vao;
            if (meshRendererComponent is GuiTextComponent)
            {
                vao = new TextVertexArrayObject();
                vao.SetRenderer(meshRendererComponent);
                vao.Upload();
                guiObjects.Add(vao);
            }
            else if (meshRendererComponent is GuiTextureComponent)
            {
                vao = new GuiVertexArrayObject();
                vao.SetRenderer(meshRendererComponent);
                vao.Upload();
                guiObjects.Add(vao);
            }
            else if (meshRendererComponent is ParticleEmitterComponent)
            {
                vao = new ParticleVertexArrayObject();
                vao.SetRenderer(meshRendererComponent);
                vao.Upload();
                worldObjects.Add(vao);
            }
            else
            { 
                vao = new MeshVertexArrayObject();
                vao.SetRenderer(meshRendererComponent);
                vao.Upload();
                worldObjects.Add(vao);
            }
        }

        public void Render()
        {
            GL.DepthFunc(DepthFunction.Less);
            GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.Multisample);
            GL.Enable(EnableCap.LineSmooth);
            GL.Enable(EnableCap.PolygonSmooth);
            GL.Enable(EnableCap.StencilTest);
            GL.Enable(EnableCap.DepthTest);
            GL.Disable(EnableCap.Blend);

            //Iterate through all world-placed objects. 
            foreach (VertexArrayObject vao in worldObjects)
            {
                vao.Render();
            }

            GL.Disable(EnableCap.CullFace);
            GL.DepthFunc(DepthFunction.Lequal);

            GL.DepthMask(false);
            skybockVertexArrayObject?.Render();
            GL.DepthMask(true);

            //Iterate through all GUI objects. This is so they render on top of everything.
            foreach (VertexArrayObject vao in guiObjects)
            {
                GL.Disable(EnableCap.DepthTest);
                GL.Enable(EnableCap.Blend);
                GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

                vao.Render();
            }
        }

        public void Dispose()
        {
        }
    }
}
