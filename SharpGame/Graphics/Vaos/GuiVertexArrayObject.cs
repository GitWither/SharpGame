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
    internal class GuiVertexArrayObject : VertexArrayObject
    {
        public MeshRendererComponent MeshRenderer { get; set; }

        public GuiVertexArrayObject(GuiTextureComponent guiTextureComponent) : base()
        {
            this.MeshRenderer = guiTextureComponent;
        }
        internal override LayerType LayerType => LayerType.GUILayer;
        public override void Upload()
        {
            this.Bind();

            this.BindVectorArrayToBuffer(this.bufferIds[0], 0, this.MeshRenderer.Mesh.Vertices, false);
            this.BindVectorArrayToBuffer(this.bufferIds[1], 1, this.MeshRenderer.Mesh.FaceTexCoords, false);

            this.BindIndices(this.MeshRenderer.Mesh.Indices);

            this.Unbind();
        }

        public override void Render()
        {
            MeshRenderer.Material.Shader.Bind();
            MeshRenderer.Material.BaseMap.Bind(TextureUnit.Texture0);

            Matrix4 transformation = MathUtil.CreateTransformationMatrix(this.MeshRenderer.Actor.PositionComponent, this.MeshRenderer.Actor.RotationComponent, this.MeshRenderer.Actor.ScaleComponent);

            MeshRenderer.Material.Shader.UploadMatrix4(SharedConstants.UniformModel, ref transformation);

            Bind();
            GL.DrawElements(BeginMode.Triangles, MeshRenderer.Mesh.Indices.Length, DrawElementsType.UnsignedInt, 0);
            Unbind();

            MeshRenderer.Material.BaseMap.Unbind();
            MeshRenderer.Material.Shader.Unbind();
        }
    }
}
