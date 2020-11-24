using SharpGame.Graphics.Meshes;
using SharpGame.Objects;
using SharpGame.Objects.Components;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpGame.Graphics
{
    public class Renderer
    {
        private List<VertexArrayObject> vertexArrayObjects;

        public Renderer()
        {
            vertexArrayObjects = new List<VertexArrayObject>();
        }

        public void AddActor(Actor actor)
        {
            MeshRendererComponent meshRendererComponent = actor.GetComponent<MeshRendererComponent>();
            if (meshRendererComponent != null)
            {
                Add(meshRendererComponent);
            }
        }

        private void Add(MeshRendererComponent meshRendererComponent)
        {
            if (meshRendererComponent.Static)
            {
                bool isAdded = false;
                for (int i = 0; i < vertexArrayObjects.Count; i++)
                {
                    if (vertexArrayObjects[i].HasRoom)
                    {
                        vertexArrayObjects[i].AddMesh(meshRendererComponent);
                        isAdded = true;
                        break;
                    }
                }
                if (!isAdded)
                {
                    VertexArrayObject vao = new VertexArrayObject();
                    vao.AddMesh(meshRendererComponent);
                    vao.Upload();
                    vertexArrayObjects.Add(vao);
                }
            }
            else
            {
                VertexArrayObject vao = new VertexArrayObject();
                vao.AddMesh(meshRendererComponent);
                vao.Upload();
                vertexArrayObjects.Add(vao);
            }
        }

        public void Render()
        {
            foreach (VertexArrayObject vao in vertexArrayObjects)
            {
                vao.Render();
            }
        }
    }
}
