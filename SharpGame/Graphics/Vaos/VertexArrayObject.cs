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

namespace SharpGame.Graphics.Vaos
{
    internal abstract class VertexArrayObject : IDisposable
    {
        private readonly int id;

        protected readonly int[] bufferIds = new int[4];


        public VertexArrayObject()
        {
            id = GL.GenVertexArray();
            GL.GenBuffers(bufferIds.Length, bufferIds);
        }

        ~VertexArrayObject()
        {
            Dispose();
        }

        public abstract void SetRenderer(MeshRendererComponent renderer);
        public abstract void Upload();

        public abstract void Render();

        protected void BindVectorArrayToBuffer(int bufferId, int attributeId, Vector3[] vectors, bool dynamic, int attribDivisor = 0)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, bufferId);
            GL.BufferData(BufferTarget.ArrayBuffer, vectors.Length * Vector3.SizeInBytes, vectors, dynamic ? BufferUsageHint.DynamicDraw : BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(attributeId, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexArrayAttrib(id, attributeId);
            GL.VertexAttribDivisor(attributeId, attribDivisor);
        }

        protected void BindVectorArrayToBuffer(int bufferId, int attributeId, Vector2[] vectors, bool dynamic, int attribDivisor = 0)
        {
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

        protected void Bind()
        {
            GL.BindVertexArray(id);
        }

        protected void Unbind()
        {
            GL.BindVertexArray(0);
        }

        public void Dispose()
        {
            //GL.DeleteBuffer(indicesId);
            //GL.DeleteBuffers(bufferIds.Length, bufferIds);
            //GL.DeleteVertexArray(id);
        }
    }
}