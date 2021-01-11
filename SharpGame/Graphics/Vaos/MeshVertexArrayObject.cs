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
    internal class MeshVertexArrayObject : VertexArrayObject
    {
        public MeshRendererComponent MeshRenderer { get; private set; }
        public override void SetRenderer(MeshRendererComponent renderer)
        {
            this.MeshRenderer = renderer;
        }
        public override void Upload()
        {
            Bind();

            BindVectorArrayToBuffer(bufferIds[0], 0, MeshRenderer.Mesh.Vertices, false);
            BindVectorArrayToBuffer(bufferIds[1], 1, MeshRenderer.Mesh.FaceTexCoords, false);
            BindVectorArrayToBuffer(bufferIds[2], 2, MeshRenderer.Mesh.Normals, false);

            BindIndices(MeshRenderer.Mesh.Indices);

            Unbind();
        }

        public override void Render()
        {
            MeshRenderer.Material.Shader.Bind();
            MeshRenderer.Material.BaseMap.Bind(TextureUnit.Texture0);
            MeshRenderer.Material.NormalMap?.Bind(TextureUnit.Texture1);
            MeshRenderer.Material.EmissionMap?.Bind(TextureUnit.Texture2);

            MeshRenderer.Material.Shader.UploadBool(SharedConstants.UniformHasNormalMap, MeshRenderer.Material.NormalMap != null);
            MeshRenderer.Material.Shader.UploadBool(SharedConstants.UniformHasEmmissionMap, MeshRenderer.Material.EmissionMap != null);

            Matrix4 modelViewProjection = SharpGameWindow.ActiveScene.Camera.View * SharpGameWindow.ActiveScene.Camera.Projection;
            Matrix4 transformation = MathUtil.CreateTransformationMatrix(this.MeshRenderer.Actor.PositionComponent, this.MeshRenderer.Actor.RotationComponent, this.MeshRenderer.Actor.ScaleComponent);

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


            Bind();
            GL.DrawElements(BeginMode.Triangles, MeshRenderer.Mesh.Indices.Length, DrawElementsType.UnsignedInt, 0);
            Unbind();

            MeshRenderer.Material.EmissionMap?.Unbind();
            MeshRenderer.Material.NormalMap?.Unbind();
            MeshRenderer.Material.BaseMap.Unbind();
            MeshRenderer.Material.Shader.Unbind();
        }
    }
}
