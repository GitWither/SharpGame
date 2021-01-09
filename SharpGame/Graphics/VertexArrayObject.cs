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
        private float time;
        private readonly int id;
        private readonly int[] bufferIds = new int[4];

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

        public void AddMesh(MeshRendererComponent meshRendererComponent)
        {
            this.MeshRenderer = meshRendererComponent;
        }

        public void Upload()
        {

            GL.BindVertexArray(id);

            BindVectorArrayToBuffer(bufferIds[0], 0, MeshRenderer.Mesh.Vertices);
            BindVectorArrayToBuffer(bufferIds[1], 1, MeshRenderer.Mesh.FaceTexCoords);
            BindVectorArrayToBuffer(bufferIds[2], 2, MeshRenderer.Mesh.Normals);

            BindIndices(MeshRenderer.Mesh.Indices);

            GL.BindVertexArray(0);
        }

        public void Render()
        {
            time += 0.1f;
            MeshRenderer.Material.Shader.UploadFloat("iTime", time);
            MeshRenderer.Material.Shader.Bind();
            MeshRenderer.Material.BaseMap.Bind(TextureUnit.Texture0);
            MeshRenderer.Material.NormalMap?.Bind(TextureUnit.Texture1);

            Matrix4 modelViewProjection = SharpGameWindow.ActiveScene.Camera.View * SharpGameWindow.ActiveScene.Camera.Projection;
            Matrix4 scale = Matrix4.CreateScale(MeshRenderer.Actor.ScaleComponent);

            Matrix4 rotation = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(MeshRenderer.Actor.RotationComponent.Roll)) *
                               Matrix4.CreateRotationY(MathHelper.DegreesToRadians(MeshRenderer.Actor.RotationComponent.Yaw)) *
                               Matrix4.CreateRotationX(MathHelper.DegreesToRadians(MeshRenderer.Actor.RotationComponent.Pitch));

            Matrix4 translation = Matrix4.CreateTranslation(this.MeshRenderer.Actor.PositionComponent);

            Matrix4 transformation = translation * rotation * scale;

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


            GL.BindVertexArray(id);
            GL.DrawElements(BeginMode.Triangles, MeshRenderer.Mesh.Indices.Length, DrawElementsType.UnsignedInt, 0);
            GL.BindVertexArray(0);

            MeshRenderer.Material.NormalMap?.Unbind();
            MeshRenderer.Material.BaseMap.Unbind();
            MeshRenderer.Material.Shader.Unbind();
        }

        private void BindVectorArrayToBuffer(int bufferId, int attributeId, Vector3[] vectors)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, bufferId);
            GL.BufferData(BufferTarget.ArrayBuffer, vectors.Length * Vector3.SizeInBytes, vectors, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(attributeId, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexArrayAttrib(id, attributeId);
        }

        private void BindVectorArrayToBuffer(int bufferId, int attributeId, Vector2[] vectors)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, bufferId);
            GL.BufferData(BufferTarget.ArrayBuffer, vectors.Length * Vector2.SizeInBytes, vectors, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(attributeId, 2, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexArrayAttrib(id, attributeId);
        }

        private void BindIndices(int[] indices)
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, bufferIds[3]);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
        }

        public void Dispose()
        {
            //GL.DeleteBuffer(indicesId);
            //GL.DeleteBuffers(bufferIds.Length, bufferIds);
            //GL.DeleteVertexArray(id);
        }
    }
}