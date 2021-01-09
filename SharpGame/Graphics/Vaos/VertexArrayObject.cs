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
    internal class VertexArrayObject : IDisposable
    {
        private float time;
        private readonly int id;

        protected readonly int[] bufferIds = new int[4];

        public MeshRendererComponent MeshRenderer { get; private set; }


        public VertexArrayObject()
        {
            id = GL.GenVertexArray();
            GL.GenBuffers(bufferIds.Length, bufferIds);
        }

        ~VertexArrayObject()
        {
            Dispose();
        }

        public virtual void AddMesh(MeshRendererComponent meshRendererComponent)
        {
            this.MeshRenderer = meshRendererComponent;
        }

        public virtual void Upload()
        {
            Bind();

            BindVectorArrayToBuffer(bufferIds[0], 0, MeshRenderer.Mesh.Vertices, false);
            BindVectorArrayToBuffer(bufferIds[1], 1, MeshRenderer.Mesh.FaceTexCoords, false);
            BindVectorArrayToBuffer(bufferIds[2], 2, MeshRenderer.Mesh.Normals, false);

            BindIndices(MeshRenderer.Mesh.Indices);

            Unbind();
        }

        public virtual void Render()
        {
            time += 0.1f;
            MeshRenderer.Material.Shader.UploadFloat("iTime", time);
            MeshRenderer.Material.Shader.Bind();
            MeshRenderer.Material.BaseMap.Bind(TextureUnit.Texture0);
            MeshRenderer.Material.NormalMap?.Bind(TextureUnit.Texture1);

            Matrix4 modelViewProjection = SharpGameWindow.ActiveScene.Camera.View * SharpGameWindow.ActiveScene.Camera.Projection;
            Matrix4 transformation = MathUtil.CreateTransformationMatrix(this.MeshRenderer.Actor.PositionComponent, this.MeshRenderer.Actor.RotationComponent, this.MeshRenderer.Actor.ScaleComponent);

            MeshRenderer.Material.Shader.UploadMatrix4(SharedConstants.UniformTransformationMatrix, ref transformation);
            MeshRenderer.Material.Shader.UploadMatrix4(SharedConstants.UniformModelViewProjection, ref modelViewProjection);

            MeshRenderer.Material.Shader.UploadFloat(SharedConstants.UniformSpecularity, MeshRenderer.Material.Specularity);

            PointLightComponent[] lights = MeshRenderer.Actor.RootScene.PointLights;

            for (int i = 0; i < SharedConstants.MaxLights; i++)
            {
                if (lights[i] != null)
                {
                    Vector3 lightPos = lights[i].Actor.PositionComponent;
                    Vector3 lightColor = lights[i].Color;
                    MeshRenderer.Material.Shader.UploadVector3($"lightPosition[{i}]", ref lightPos);
                    MeshRenderer.Material.Shader.UploadVector3($"lightColor[{i}]", ref lightColor);
                }
            }


            Bind();
            GL.DrawElements(BeginMode.Triangles, MeshRenderer.Mesh.Indices.Length, DrawElementsType.UnsignedInt, 0);
            Unbind();

            MeshRenderer.Material.NormalMap?.Unbind();
            MeshRenderer.Material.BaseMap.Unbind();
            MeshRenderer.Material.Shader.Unbind();
        }

        protected void BindVectorArrayToBuffer(int bufferId, int attributeId, Vector3[] vectors, bool dynamic)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, bufferId);
            GL.BufferData(BufferTarget.ArrayBuffer, vectors.Length * Vector3.SizeInBytes, vectors, dynamic ? BufferUsageHint.DynamicDraw : BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(attributeId, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexArrayAttrib(id, attributeId);
        }

        protected void BindVectorArrayToBuffer(int bufferId, int attributeId, Vector2[] vectors, bool dynamic)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, bufferId);
            GL.BufferData(BufferTarget.ArrayBuffer, vectors.Length * Vector2.SizeInBytes, vectors, dynamic ? BufferUsageHint.DynamicDraw : BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(attributeId, 2, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexArrayAttrib(id, attributeId);
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