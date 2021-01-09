using OpenTK;
using OpenTK.Graphics.OpenGL4;

using SharpGame.Util;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpGame.Graphics.Vaos
{
    class ParticleVertexArrayObject : VertexArrayObject
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
            Matrix4 transformation = MathUtil.CreateTransformationMatrix(this.MeshRenderer.Actor.PositionComponent, this.MeshRenderer.Actor.RotationComponent, this.MeshRenderer.Actor.ScaleComponent);

            MeshRenderer.Material.Shader.UploadMatrix4(SharedConstants.UniformModelViewProjection, ref modelViewProjection);
            MeshRenderer.Material.Shader.UploadMatrix4(SharedConstants.UniformTransformationMatrix, ref transformation);

            Bind();
            //GL.DrawElements(BeginMode.Triangles, MeshRenderer.Mesh.Indices.Length, DrawElementsType.UnsignedInt, 0);
            GL.DrawElementsInstanced(PrimitiveType.Triangles, MeshRenderer.Mesh.Indices.Length, DrawElementsType.UnsignedInt, (IntPtr)0, 2555);
            Unbind();

            MeshRenderer.Material.BaseMap.Unbind();
            MeshRenderer.Material.Shader.Unbind();
        }
    }
}
