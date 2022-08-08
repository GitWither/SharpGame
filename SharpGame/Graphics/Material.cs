using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using SharpGame.Graphics.Vaos;
using SharpGame.Util;

namespace SharpGame.Graphics
{
    public class Material : IDisposable
    {
        public Shader Shader { get; set; }
        public Texture BaseMap { get; set; }
        public Texture NormalMap { get; set; }
        public Texture EmissionMap { get; set; }
        public float Specularity { get; set; }

        public Material(Shader shader, Texture baseMap) : this(shader, baseMap, null, null, 0.0f)
        {
        }

        public Material(Shader shader, Texture baseMap, Texture normalMap) : this(shader, baseMap, normalMap, null, 0.0f)
        {
        }

        public Material(Shader shader, Texture baseMap, Texture normalMap, Texture emissionMap) : this(shader, baseMap, normalMap, emissionMap, 0.0f)
        {

        }

        public Material(Shader shader, Texture baseMap, float specularity) : this(shader, baseMap, null, null, specularity)
        {
        }
        
        public Material(Shader shader, Texture baseMap, Texture normalMap, Texture emissionMap, float specularity)
        {
            this.Shader = shader;
            this.BaseMap = baseMap;
            this.NormalMap = normalMap;
            this.EmissionMap = emissionMap;
            this.Specularity = specularity;
        }


        public void Dispose()
        {
            Shader?.Dispose();
            BaseMap?.Dispose();
            NormalMap?.Dispose();
            EmissionMap?.Dispose();
        }

        public void Draw(VertexArrayObject vao, Matrix4 transformation)
        {
            Shader.Bind();
            BaseMap.Bind(TextureUnit.Texture0);

            Shader.UploadBool(SharedConstants.UniformHasNormalMap, false);
            Shader.UploadBool(SharedConstants.UniformHasEmmissionMap, false);

            //var sceneProjection = SharpGameWindow.ActiveScene.Projection;
            //var sceneView = SharpGameWindow.ActiveScene.View;
            //Shader.UploadMatrix4(SharedConstants.UniformView, ref sceneView);
            //Shader.UploadMatrix4(SharedConstants.UniformProjection, ref sceneProjection);
            //Shader.UploadMatrix4(SharedConstants.UniformModel, ref transformation);

            Shader.UploadFloat(SharedConstants.UniformSpecularity, Specularity);

            vao.Bind();
            GL.DrawElements(BeginMode.Triangles, vao.indices.Length, DrawElementsType.UnsignedInt, 0);
        }
    }
}
