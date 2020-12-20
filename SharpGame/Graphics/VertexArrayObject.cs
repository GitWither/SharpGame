using OpenTK;
using OpenTK.Graphics.OpenGL;

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

        private Vector3 translation;

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
            translation = meshRendererComponent.Actor.PositionComponent;
            if (meshCount < SharedConstants.MaxMeshes)
            {
                this.AddVertices(meshRendererComponent.Mesh.Vertices);
                this.AddIndices(meshRendererComponent.Mesh.FaceIndices);
                this.AddTexCoords(meshRendererComponent.Mesh.FaceTexCoords);
                this.meshRendererComponents.Add(meshRendererComponent);


                Matrix4 transformMatrix = Matrix4.CreateTranslation(meshRendererComponent.Actor.PositionComponent.X, meshRendererComponent.Actor.PositionComponent.Y, meshRendererComponent.Actor.PositionComponent.Z);
                for (int i = 0; i < vertices.Count; i++)
                {
                    Vector4 pos = new Vector4(vertices[i], 1);
                    pos = transformMatrix * pos;
                    vertices[i] = new Vector3(pos.Xyz);
                }

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

            GL.BindBuffer(BufferTarget.ArrayBuffer, bufferIds[2]);
            GL.BufferData(BufferTarget.ArrayBuffer, normals.Count * Vector3.SizeInBytes, normals.ToArray(), BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexArrayAttrib(id, 2);


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
            //GL.Disable(EnableCap.CullFace);
            shader.Bind();
            translation = this.meshRendererComponents[0].Actor.PositionComponent;
            Matrix4 modelViewProjection = meshRendererComponents[0].Actor.RootScene.Camera.View * meshRendererComponents[0].Actor.RootScene.Camera.Projection;
            Matrix4 scale = Matrix4.CreateScale(meshRendererComponents[0].Actor.ScaleComponent);

            Matrix4 rotation = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(meshRendererComponents[0].Actor.RotationComponent.Pitch)) *
                               Matrix4.CreateRotationY(MathHelper.DegreesToRadians(meshRendererComponents[0].Actor.RotationComponent.Yaw)) *
                               Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(meshRendererComponents[0].Actor.RotationComponent.Roll));

            Matrix4 translationMatrix = Matrix4.CreateTranslation(translation);

            Matrix4 transformation = translationMatrix * rotation * scale;

            shader.UploadMatrix4(SharedConstants.UniformTranslationMatrix, ref transformation);
            shader.UploadMatrix4(SharedConstants.UniformModelViewProjection, ref modelViewProjection);

            translation = Vector3.Zero;

            GL.BindVertexArray(id);
            GL.DrawElements(BeginMode.Triangles, count, DrawElementsType.UnsignedInt, 0);

            //GL.DisableVertexArrayAttrib(id, 0);
            //GL.DisableVertexArrayAttrib(id, 1);
            //GL.DisableVertexArrayAttrib(id, 2);
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
