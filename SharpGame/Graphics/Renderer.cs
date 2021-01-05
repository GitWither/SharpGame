using OpenTK.Audio.OpenAL;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

using SharpGame.Graphics.Meshes;
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
    public class Renderer : IDisposable
    {
        private List<VertexArrayObject> vertexArrayObjects;

        public Renderer()
        {
            vertexArrayObjects = new List<VertexArrayObject>();
        }

        ~Renderer()
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
            VertexArrayObject vao = new VertexArrayObject();
            vao.AddMesh(meshRendererComponent);
            vao.Upload();
            vertexArrayObjects.Add(vao);
        }

        public void Render()
        {
            GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.LineSmooth);
            GL.Enable(EnableCap.PolygonSmooth);
            GL.Enable(EnableCap.StencilTest);
            //Logger.Info(vertexArrayObjects.Count);
            foreach (VertexArrayObject vao in vertexArrayObjects)
            {

                if (vao.MeshRenderer is GuiTextComponent component)
                {
                    vao.MeshRenderer.Mesh = Mesh.FromText(component.Text);
                    vao.Upload();

                    GL.Disable(EnableCap.DepthTest);
                    GL.Enable(EnableCap.Blend);
                    GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
                }

                if (vao.MeshRenderer is GuiTextureComponent)
                {
                    GL.Disable(EnableCap.DepthTest);
                    GL.Enable(EnableCap.Blend);
                    GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
                }

                vao.Render();

                GL.Enable(EnableCap.DepthTest);
                GL.Disable(EnableCap.Blend);
            }
        }

        public void Dispose()
        {
        }
    }
}
