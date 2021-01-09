using OpenTK;
using OpenTK.Graphics.OpenGL4;

using SharpGame.Graphics.Meshes;
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
        public GuiTextComponent TextRenderer { get; set; }

        public override void SetRenderer(MeshRendererComponent renderer)
        {
            this.TextRenderer = (GuiTextComponent)renderer;
        }
        public override void Upload()
        {
            this.Bind();

            this.BindVectorArrayToBuffer(this.bufferIds[0], 0, this.TextRenderer.Mesh.Vertices, true);
            this.BindVectorArrayToBuffer(this.bufferIds[1], 1, this.TextRenderer.Mesh.FaceTexCoords, true);

            this.BindIndices(this.TextRenderer.Mesh.Indices);

            this.Unbind();
        }

        public override void Render()
        {
            if (TextRenderer.Rebuffer)
            {
                this.Upload();
                TextRenderer.Rebuffer = false;
            }

            TextRenderer.Material.Shader.Bind();
            TextRenderer.Material.BaseMap.Bind(TextureUnit.Texture0);

            Matrix4 transformation = MathUtil.CreateTransformationMatrix(this.TextRenderer.Actor.PositionComponent, this.TextRenderer.Actor.RotationComponent, this.TextRenderer.Actor.ScaleComponent);

            TextRenderer.Material.Shader.UploadMatrix4(SharedConstants.UniformTransformationMatrix, ref transformation);

            Bind();
            GL.DrawElements(BeginMode.Triangles, TextRenderer.Mesh.Indices.Length, DrawElementsType.UnsignedInt, 0);
            Unbind();

            TextRenderer.Material.BaseMap.Unbind();
            TextRenderer.Material.Shader.Unbind();
        }
    }
}
