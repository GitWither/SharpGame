using OpenTK;
using OpenTK.Graphics.OpenGL4;

using SharpGame.Graphics.Meshes;
using SharpGame.Util;

namespace SharpGame.Graphics.Vaos
{
    internal class SkybockVertexArrayObject : VertexArrayObject
    {
        private SkyboxMaterial Material { get; set; }

        internal override LayerType LayerType => LayerType.GUILayer;
        public void SetSkyboxMaterial(SkyboxMaterial skyboxMaterial)
        {
            this.Material = skyboxMaterial;
        }

        public override void Upload()
        {
            Bind();
            BindVectorArrayToBuffer(bufferIds[0], 0, Mesh.SkyBox.Vertices, false);

            BindIndices(Mesh.SkyBox.Indices);

            Unbind();
        }
        public override void Render()
        {
            this.Material.Shader.Bind();
            this.Material.Cubemap.Bind(TextureUnit.Texture0);

            Matrix4 view = SharpGameWindow.ActiveScene.Camera.View.ClearTranslation();
            Matrix4 projection = SharpGameWindow.ActiveScene.Camera.Projection;

            view *= Matrix4.CreateTranslation(0, 0, 0);

            Material.Shader.UploadMatrix4(SharedConstants.UniformView, ref view);
            Material.Shader.UploadMatrix4(SharedConstants.UniformProjection, ref projection);

            Bind();
            GL.DrawElements(BeginMode.Triangles, Mesh.SkyBox.Indices.Length, DrawElementsType.UnsignedInt, 0);
            Unbind();

            this.Material.Cubemap.Unbind();
            this.Material.Shader.Unbind();
        }
    }
}
