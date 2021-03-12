using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

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

        public MeshVertexArrayObject(MeshRendererComponent meshRendererComponent) : base()
        {
            this.MeshRenderer = meshRendererComponent;
        }

        internal override LayerType LayerType => LayerType.WorldLayer;

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

            Matrix4 view = SharpGameWindow.ActiveScene.Camera.View;
            Matrix4 projection = SharpGameWindow.ActiveScene.Camera.Projection;
            Matrix4 transformation = MathUtil.CreateTransformationMatrix(this.MeshRenderer.Actor);

            MeshRenderer.Material.Shader.UploadMatrix4(SharedConstants.UniformView, ref view);
            MeshRenderer.Material.Shader.UploadMatrix4(SharedConstants.UniformProjection, ref projection);
            MeshRenderer.Material.Shader.UploadMatrix4(SharedConstants.UniformModel, ref transformation);

            MeshRenderer.Material.Shader.UploadFloat(SharedConstants.UniformSpecularity, MeshRenderer.Material.Specularity);

            PointLightComponent[] lights = MeshRenderer.Actor.RootScene.PointLights;

            Span<Vector3> lightPositions = stackalloc Vector3[SharedConstants.MaxLights];
            Span<Vector3> lightColors = stackalloc Vector3[SharedConstants.MaxLights];
            Span<float> lightDistances = stackalloc float[SharedConstants.MaxLights];

            for (int i = 0; i < SharedConstants.MaxLights; i++)
            {
                if (lights[i] == null) continue;

                lightPositions[i] = lights[i].Actor.PositionComponent;
                lightColors[i] = lights[i].Color;
                lightDistances[i] = lights[i].MaxDistance;
            }

            MeshRenderer.Material.Shader.UploadFloatArray($"lightDistance", lightDistances);
            MeshRenderer.Material.Shader.UploadVector3Array($"lightPosition", lightPositions);
            MeshRenderer.Material.Shader.UploadVector3Array($"lightColor", lightColors);


            Bind();
            GL.DrawElements(BeginMode.Triangles, MeshRenderer.Mesh.Indices.Length, DrawElementsType.UnsignedInt, 0);
            Unbind();

            MeshRenderer.Material.BaseMap.Unbind();
            MeshRenderer.Material.Shader.Unbind();
        }
    }
}
