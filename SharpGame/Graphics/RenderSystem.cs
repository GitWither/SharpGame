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

        private void Add(MeshRendererComponent meshRendererComponent)
        {
            VertexArrayObject vao;
            if (meshRendererComponent is GuiTextComponent)
            {
                vao = new TextVertexArrayObject();
                vao.AddMesh(meshRendererComponent);
                vao.Upload();
                guiObjects.Add(vao);
            }
            else if (meshRendererComponent is GuiTextureComponent)
            {
                vao = new GuiVertexArrayObject();
                vao.AddMesh(meshRendererComponent);
                vao.Upload();
                guiObjects.Add(vao);
            }
            else if (meshRendererComponent is ParticleEmitterComponent)
            {
                vao = new ParticleVertexArrayObject();
                vao.AddMesh(meshRendererComponent);
                vao.Upload();
                worldObjects.Add(vao);
            }
            else
            { 
                vao = new VertexArrayObject();
                vao.AddMesh(meshRendererComponent);
                vao.Upload();
                worldObjects.Add(vao);
            }
        }

        public void Render()
        {
            GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.LineSmooth);
            GL.Enable(EnableCap.PolygonSmooth);
            GL.Enable(EnableCap.StencilTest);

            //Iterate through all world-placed objects. 
            foreach (VertexArrayObject vao in worldObjects)
            {
                vao.Render();
            }
            
            //Iterate through all GUI objects. This is so they render on top of everything.
            foreach (VertexArrayObject vao in guiObjects)
            {
                GL.Disable(EnableCap.DepthTest);
                GL.Enable(EnableCap.Blend);
                GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

                if (vao.MeshRenderer is GuiTextComponent component)
                {
                    vao.MeshRenderer.Mesh = Mesh.FromText(component.Text);
                    vao.Upload();
                }

                vao.Render();
            }

            GL.Enable(EnableCap.DepthTest);
            GL.Disable(EnableCap.Blend);
        }

        public void Dispose()
        {
        }
    }
}
