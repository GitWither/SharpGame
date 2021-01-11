﻿using OpenTK;
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
                new Vector3(3, 1, 7)
            }, true);
            GL.VertexAttribDivisor(2, 1);

            this.BindIndices(Mesh.GuiQuad.Indices);

            this.Unbind();
        }
        public override void Render()
        {
            ParticleEmitter.Material.Shader.Bind();
            ParticleEmitter.Material.BaseMap.Bind(TextureUnit.Texture0);

            Matrix4 modelViewProjection = SharpGameWindow.ActiveScene.Camera.View * SharpGameWindow.ActiveScene.Camera.Projection;
            Matrix4 view = SharpGameWindow.ActiveScene.Camera.View;
            Matrix4 transformation = MathUtil.CreateTransformationMatrix(
                this.ParticleEmitter.Actor.PositionComponent, 
                this.ParticleEmitter.Actor.RotationComponent, 
                this.ParticleEmitter.Actor.ScaleComponent
                );


            //modelViewProjection *= Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(SharpGameWindow.ActiveScene.Camera.Actor.RotationComponent.Yaw));
            //modelViewProjection *= Matrix4.CreateScale(2);

            //modelViewProjection *= SharpGameWindow.ActiveScene.Camera.View;

            ParticleEmitter.Material.Shader.UploadMatrix4("view", ref view);
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
