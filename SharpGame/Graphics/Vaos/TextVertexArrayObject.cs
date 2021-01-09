using OpenTK;
using OpenTK.Graphics.OpenGL4;

using SharpGame.Objects.Components;
using SharpGame.Util;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpGame.Graphics.Vaos
{
    internal class TextVertexArrayObject : VertexArrayObject
    {
        public override void Upload()
        {
            this.Bind();

            this.BindVectorArrayToBuffer(this.bufferIds[0], 0, this.MeshRenderer.Mesh.Vertices, true);
            this.BindVectorArrayToBuffer(this.bufferIds[1], 1, this.MeshRenderer.Mesh.FaceTexCoords, true);

            this.BindIndices(this.MeshRenderer.Mesh.Indices);

            this.Unbind();
        }

        public override void Render()
        {
            MeshRenderer.Material.Shader.Bind();
            MeshRenderer.Material.BaseMap.Bind(TextureUnit.Texture0);

            Matrix4 modelViewProjection = SharpGameWindow.ActiveScene.Camera.View * SharpGameWindow.ActiveScene.Camera.Projection;
            Matrix4 scale = Matrix4.CreateScale(MeshRenderer.Actor.ScaleComponent);

            Matrix4 rotation = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(MeshRenderer.Actor.RotationComponent.Roll)) *
                               Matrix4.CreateRotationY(MathHelper.DegreesToRadians(MeshRenderer.Actor.RotationComponent.Yaw)) *
                               Matrix4.CreateRotationX(MathHelper.DegreesToRadians(MeshRenderer.Actor.RotationComponent.Pitch));

            Matrix4 translation = Matrix4.CreateTranslation(this.MeshRenderer.Actor.PositionComponent);

            Matrix4 transformation = translation * rotation * scale;

            MeshRenderer.Material.Shader.UploadMatrix4(SharedConstants.UniformTransformationMatrix, ref transformation);
            MeshRenderer.Material.Shader.UploadMatrix4(SharedConstants.UniformModelViewProjection, ref modelViewProjection);

            Bind();
            GL.DrawElements(BeginMode.Triangles, MeshRenderer.Mesh.Indices.Length, DrawElementsType.UnsignedInt, 0);
            Unbind();

            MeshRenderer.Material.BaseMap.Unbind();
            MeshRenderer.Material.Shader.Unbind();
        }
    }
}
