using OpenTK;
using OpenTK.Graphics.OpenGL4;

using SharpGame.Graphics.Meshes;
using SharpGame.Objects.Components;
using SharpGame.Particles;
using SharpGame.Util;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpGame.Graphics.Vaos
{
    internal class ParticleVertexArrayObject : VertexArrayObject
    {
        Random random = new Random();
        private ParticleEmitterComponent ParticleEmitter { get; set; }

        public ParticleVertexArrayObject(ParticleEmitterComponent particleEmitterComponent) : base()
        {
            this.ParticleEmitter = particleEmitterComponent;
        }

        internal override LayerType LayerType => LayerType.WorldLayer;

        Particle[] particlePool;
        public override void Upload()
        {
            particlePool = new Particle[ParticleEmitter.Count];

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

            Matrix4 view = SharpGameWindow.ActiveScene.Camera.View;
            Matrix4 projection = SharpGameWindow.ActiveScene.Camera.Projection;
            Matrix4 transformation = MathUtil.CreateTransformationMatrix(this.ParticleEmitter.Actor);

            Span<Vector3> positions = stackalloc Vector3[ParticleEmitter.Count];
            for (int i = 0; i < ParticleEmitter.Count; i++)
            {
                Vector3 particlePos = particlePool[i].Position;


                particlePos.X += 0;
                particlePos.Y += ParticleEmitter.Velocity;
                particlePos.Z += 0;

                particlePool[i].Lifetime++;
                positions[i] = particlePos;
                particlePool[i].Position = particlePos;

                if (particlePool[i].Lifetime == ParticleEmitter.Life)
                {
                    particlePool[i].Reset();
                }
            }

            this.Bind();
            this.BindVectorArrayToBuffer(this.bufferIds[2], 2, positions.ToArray(), true, 1);

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
