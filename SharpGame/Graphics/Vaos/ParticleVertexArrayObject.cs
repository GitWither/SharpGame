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
    internal class ParticleVertexArrayObject : VertexArrayObject
    {
        Random random = new Random();
        private ParticleEmitterComponent ParticleEmitter { get; set; }
        public override void SetRenderer(MeshRendererComponent renderer)
        {
            this.ParticleEmitter = (ParticleEmitterComponent)renderer;
        }
        Vector3[] positions;
        public override void Upload()
        {
            positions = new Vector3[ParticleEmitter.Count];
            this.Bind();

            this.BindVectorArrayToBuffer(this.bufferIds[0], 0, Mesh.GuiQuad.Vertices, false);
            this.BindVectorArrayToBuffer(this.bufferIds[1], 1, Mesh.GuiQuad.FaceTexCoords, false);

            this.BindVectorArrayToBuffer(this.bufferIds[2], 2, new Vector3[] {
                new Vector3(2, 2, 5),
                new Vector3(1, 2, 5),
                new Vector3(5, 7, 2),
                new Vector3(2, 6, 5),
                new Vector3(2, 3, 6),
                new Vector3(5, 6, 2),
                new Vector3(6, 5, 6),
                new Vector3(8, 2, 2),
                new Vector3(3, 5, 8),
                new Vector3(6, 4, 9),
                new Vector3(7, 2, 6),
                new Vector3(3, 1, 7),
                new Vector3(3, 1, 7)
            }, true, 1);

            this.BindIndices(Mesh.GuiQuad.Indices);

            this.Unbind();
        }
        public override void Render()
        {
            ParticleEmitter.Material.Shader.Bind();
            ParticleEmitter.Material.BaseMap.Bind(TextureUnit.Texture0);

            Matrix4 view = SharpGameWindow.ActiveScene.Camera.View;
            Matrix4 projection = SharpGameWindow.ActiveScene.Camera.Projection;
            Matrix4 transformation = MathUtil.CreateTransformationMatrix(
                this.ParticleEmitter.Actor.PositionComponent, 
                this.ParticleEmitter.Actor.RotationComponent, 
                this.ParticleEmitter.Actor.ScaleComponent
                );

            for (int i = 0; i < ParticleEmitter.Count; i++)
            {
                positions[i].X += 0.001f;
                positions[i].Y += 0.001f ;
                positions[i].Z += 0.001f;
            }

            this.Bind();
            this.BindVectorArrayToBuffer(this.bufferIds[2], 2, positions, true, 1);

            ParticleEmitter.Material.Shader.UploadMatrix4(SharedConstants.UniformView, ref view);
            ParticleEmitter.Material.Shader.UploadMatrix4(SharedConstants.UniformProjection, ref projection);
            ParticleEmitter.Material.Shader.UploadMatrix4(SharedConstants.UniformModel, ref transformation);

            GL.DrawElementsInstanced(PrimitiveType.Triangles, Mesh.GuiQuad.Indices.Length, DrawElementsType.UnsignedInt, (IntPtr)0, ParticleEmitter.Count);
            Unbind();

            ParticleEmitter.Material.BaseMap.Unbind();
            ParticleEmitter.Material.Shader.Unbind();
        }
    }
}
