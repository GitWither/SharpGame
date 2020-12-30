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

        public MeshRendererComponent MeshRenderer { get; private set; }


        public VertexArrayObject()
        {
            id = GL.GenVertexArray();
            GL.GenBuffers(bufferIds.Length, bufferIds);
            indicesId = GL.GenBuffer();
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
            count = MeshRenderer.Mesh.Indices.Length;

            GL.BindVertexArray(id);

            GL.BindBuffer(BufferTarget.ArrayBuffer, bufferIds[0]);
            GL.BufferData(BufferTarget.ArrayBuffer, MeshRenderer.Mesh.Vertices.Length * Vector3.SizeInBytes, MeshRenderer.Mesh.Vertices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, bufferIds[1]);
            GL.BufferData(BufferTarget.ArrayBuffer, MeshRenderer.Mesh.FaceTexCoords.Length * Vector2.SizeInBytes, MeshRenderer.Mesh.FaceTexCoords, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, bufferIds[2]);
            GL.BufferData(BufferTarget.ArrayBuffer, MeshRenderer.Mesh.Normals.Length * Vector3.SizeInBytes, MeshRenderer.Mesh.Normals, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 0, 0);


            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indicesId);
            GL.BufferData(BufferTarget.ElementArrayBuffer, MeshRenderer.Mesh.Indices.Length * sizeof(uint), MeshRenderer.Mesh.Indices, BufferUsageHint.StaticDraw);

            GL.EnableVertexArrayAttrib(id, 0);
            GL.EnableVertexArrayAttrib(id, 1);
            GL.EnableVertexArrayAttrib(id, 2);
        }

        public void Render()
        {

            MeshRenderer.Material.Shader.Bind();
            MeshRenderer.Material.BaseMap.Bind(TextureUnit.Texture0);
            MeshRenderer.Material.NormalMap?.Bind(TextureUnit.Texture1);

            Matrix4 modelViewProjection = SharpGameWindow.ActiveScene.Camera.View * SharpGameWindow.ActiveScene.Camera.Projection;
            Matrix4 scale = Matrix4.CreateScale(MeshRenderer.Actor.ScaleComponent);

            Matrix4 rotation = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(MeshRenderer.Actor.RotationComponent.Pitch)) *
                               Matrix4.CreateRotationY(MathHelper.DegreesToRadians(MeshRenderer.Actor.RotationComponent.Yaw)) *
                               Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(MeshRenderer.Actor.RotationComponent.Roll));

            Matrix4 translation = Matrix4.CreateTranslation(this.MeshRenderer.Actor.PositionComponent);

            Matrix4 transformation = translation * rotation * scale;

            MeshRenderer.Material.Shader.UploadMatrix4(SharedConstants.UniformTranslationMatrix, ref transformation);
            MeshRenderer.Material.Shader.UploadMatrix4(SharedConstants.UniformModelViewProjection, ref modelViewProjection);

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
            GL.DrawElements(BeginMode.Triangles, count, DrawElementsType.UnsignedInt, 0);
            //GL.DrawArrays(PrimitiveType.Triangles, 0, MeshRenderer.Mesh.Vertices.Length);
            GL.BindVertexArray(0);

            MeshRenderer.Material.NormalMap?.Unbind();
            MeshRenderer.Material.BaseMap.Unbind();
            MeshRenderer.Material.Shader.Unbind();
        }

        public void Dispose()
        {
            //GL.DeleteBuffer(indicesId);
            //GL.DeleteBuffers(bufferIds.Length, bufferIds);
            //GL.DeleteVertexArray(id);
        }
    }
}