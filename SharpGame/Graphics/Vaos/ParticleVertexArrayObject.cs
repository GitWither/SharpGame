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
    class ParticleVertexArrayObject : VertexArrayObject
    {
        private ParticleEmitterComponent ParticleEmitter { get; set; }
        public override void SetRenderer(MeshRendererComponent renderer)
        {
            this.ParticleEmitter = (ParticleEmitterComponent)renderer;
        }
        public override void Upload()
        {
            this.Bind();

            this.BindVectorArrayToBuffer(this.bufferIds[0], 0, Mesh.GuiQuad.Vertices, false);
            this.BindVectorArrayToBuffer(this.bufferIds[1], 1, Mesh.GuiQuad.FaceTexCoords, false);

            this.BindIndices(Mesh.GuiQuad.Indices);

            this.Unbind();
        }
        public override void Render()
        {
            ParticleEmitter.Material.Shader.Bind();
            ParticleEmitter.Material.BaseMap.Bind(TextureUnit.Texture0);

            Matrix4 modelViewProjection = SharpGameWindow.ActiveScene.Camera.View * SharpGameWindow.ActiveScene.Camera.Projection;
            Matrix4 transformation = MathUtil.CreateTransformationMatrix(
                this.ParticleEmitter.Actor.PositionComponent, 
                this.ParticleEmitter.Actor.RotationComponent, 
                this.ParticleEmitter.Actor.ScaleComponent
                );

            ParticleEmitter.Material.Shader.UploadMatrix4(SharedConstants.UniformModelViewProjection, ref modelViewProjection);
            ParticleEmitter.Material.Shader.UploadMatrix4(SharedConstants.UniformTransformationMatrix, ref transformation);

            Bind();
            //GL.DrawElements(BeginMode.Triangles, MeshRenderer.Mesh.Indices.Length, DrawElementsType.UnsignedInt, 0);
            GL.DrawElementsInstanced(PrimitiveType.Triangles, Mesh.GuiQuad.Indices.Length, DrawElementsType.UnsignedInt, (IntPtr)0, ParticleEmitter.Count);
            Unbind();

            ParticleEmitter.Material.BaseMap.Unbind();
            ParticleEmitter.Material.Shader.Unbind();
        }
    }
}
