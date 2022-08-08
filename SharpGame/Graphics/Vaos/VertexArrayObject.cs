using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

using SharpGame.Graphics.Meshes;
using SharpGame.Objects.Components;
using SharpGame.Util;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SharpGame.Graphics.Vaos
{
    public class VertexArrayObject : IDisposable
    {
        private readonly int id;

        protected readonly int[] bufferIds = new int[4];

        public Vector3[] vertices;
        public Vector2[] uv;
        public Vector3[] normals;
        public int[] indices;

        public VertexArrayObject()
        {
            id = GL.GenVertexArray();
            GL.GenBuffers(bufferIds.Length, bufferIds);
        }

        public void Upload(Vector3[] vertices, Vector2[] uv, Vector3[] normals, int[] indices)
        {
            this.vertices = vertices;
            this.uv = uv;
            this.normals = normals;
            this.indices = indices;
            Bind();

            BindVectorArrayToBuffer(bufferIds[0], 0, vertices, false);
            BindVectorArrayToBuffer(bufferIds[1], 1, uv, false);
            BindVectorArrayToBuffer(bufferIds[2], 2, normals, false);

            BindIndices(indices);

            Unbind();
        }


        protected void BindVectorArrayToBuffer(int bufferId, int attributeId, Vector3[] vectors, bool dynamic, int attribDivisor = 0)
        {
            if (vectors == null) return;

            GL.BindBuffer(BufferTarget.ArrayBuffer, bufferId);
            GL.BufferData(BufferTarget.ArrayBuffer, vectors.Length * Vector3.SizeInBytes, vectors, dynamic ? BufferUsageHint.DynamicDraw : BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(attributeId, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexArrayAttrib(id, attributeId);
            GL.VertexAttribDivisor(attributeId, attribDivisor);
        }

        protected void BindVectorArrayToBuffer(int bufferId, int attributeId, Vector2[] vectors, bool dynamic, int attribDivisor = 0)
        {
            if (vectors == null) return;

            GL.BindBuffer(BufferTarget.ArrayBuffer, bufferId);
            GL.BufferData(BufferTarget.ArrayBuffer, vectors.Length * Vector2.SizeInBytes, vectors, dynamic ? BufferUsageHint.DynamicDraw : BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(attributeId, 2, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexArrayAttrib(id, attributeId);
            GL.VertexAttribDivisor(attributeId, attribDivisor);
        }

        protected void BindIndices(int[] indices)
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, bufferIds[3]);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
        }

        public void Bind()
        {
            GL.BindVertexArray(id);
        }

        protected void Unbind()
        {
            GL.BindVertexArray(0);
        }

        public void Dispose()
        {
            GL.DeleteVertexArray(id);
            GL.DeleteBuffers(bufferIds.Length, bufferIds);
        }
    }
}