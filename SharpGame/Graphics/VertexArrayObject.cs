using OpenTK;
using OpenTK.Graphics.OpenGL4;

using SharpGame.Graphics.Meshes;
using SharpGame.Objects.Components;
using SharpGame.Util;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SharpGame.Graphics
{
    internal class VertexArrayObject : IDisposable
    {
        private readonly int id;
        private readonly int[] bufferIds = new int[3];
        private readonly int indicesId;

        private int count;

        private readonly List<Vector3> vertices = new List<Vector3>();
        private readonly List<Vector3> normals = new List<Vector3>();
        private readonly List<Vector2> texCoords = new List<Vector2>();
        private readonly List<uint> indices = new List<uint>();

        private readonly Shader shader;

        private List<MeshRendererComponent> meshRendererComponents;

        private int meshCount;

        public bool HasRoom { get; private set; }

        public Vector3[] Vertices
        {
            get
            {
                return vertices.ToArray();
            }
        }
        public Vector2[] TexCoords
        {
            get
            {
                return texCoords.ToArray();
            }
        }

        public Vector3[] Normals
        {
            get
            {
                return normals.ToArray();
            }
        }
        public uint[] Indices
        {
            get
            {
                return indices.ToArray();
            }
        }


        public VertexArrayObject()
        {
            id = GL.GenVertexArray();
            GL.GenBuffers(bufferIds.Length, bufferIds);
            indicesId = GL.GenBuffer();

            meshRendererComponents = new List<MeshRendererComponent>();
            HasRoom = true;

            shader = new Shader("shader");
        }

        ~VertexArrayObject()
        {
            //Dispose();
        }

        public void AddVertices(Vector3[] vertices)
        {
            this.vertices.AddRange(vertices);
        }

        public void AddNormals(Vector3[] normals)
        {
            this.normals.AddRange(normals);
        }

        public void AddTexCoords(Vector2[] texCoords)
        {
            this.texCoords.AddRange(texCoords);
        }

        public void AddIndices(uint[] indices)
        {
            this.indices.AddRange(indices);
        }

        public void AddMesh(MeshRendererComponent meshRendererComponent)
        {
            if (meshCount < SharedConstants.MaxMeshes)
            {
                this.AddVertices(meshRendererComponent.Mesh.Vertices);
                this.AddIndices(meshRendererComponent.Mesh.FaceIndices);
                this.AddTexCoords(meshRendererComponent.Mesh.FaceTexCoords);
                this.meshRendererComponents.Add(meshRendererComponent);
                meshCount++;
            }
            else
            {
                HasRoom = false;
            }
        }

        public void Upload()
        {
            count = indices.Count;

            GL.BindVertexArray(id);

            GL.BindBuffer(BufferTarget.ArrayBuffer, bufferIds[0]);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Count * Vector3.SizeInBytes, vertices.ToArray(), BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexArrayAttrib(id, 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, bufferIds[1]);
            GL.BufferData(BufferTarget.ArrayBuffer, texCoords.Count * Vector2.SizeInBytes, texCoords.ToArray(), BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexArrayAttrib(id, 1);

            //GL.BindBuffer(BufferTarget.ArrayBuffer, bufferIds[2]);
            //GL.BufferData(BufferTarget.ArrayBuffer, normals.Count * Vector3.SizeInBytes, normals.ToArray(), BufferUsageHint.StaticDraw);
            //GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 0, 0);
            //GL.EnableVertexArrayAttrib(id, 2);


            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indicesId);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Count * sizeof(uint), indices.ToArray(), BufferUsageHint.StaticDraw);
        }

        public void Clear()
        {
            vertices.Clear();
            texCoords.Clear();
            normals.Clear();
            indices.Clear();
        }

        public void Render()
        {

            Matrix4 modelViewProjection = meshRendererComponents[0].Actor.RootScene.Camera.View * meshRendererComponents[0].Actor.RootScene.Camera.Projection;
            GL.UniformMatrix4(0, false, ref modelViewProjection);
            shader.Bind();

            GL.BindVertexArray(id);
            //GL.DrawArrays(PrimitiveType.Triangles, 0, vertices.Count);
            GL.DrawElements(BeginMode.Triangles, count, DrawElementsType.UnsignedInt, 0);
        }

        public void Dispose()
        {
            GL.DeleteBuffer(indicesId);
            GL.DeleteBuffers(bufferIds.Length, bufferIds);
            GL.DeleteVertexArray(id);

            Clear();
        }
    }
}
