using OpenTK.Graphics.OpenGL4;

using SharpGame.Graphics;
using SharpGame.Graphics.Meshes;
using SharpGame.Graphics.Vaos;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpGame.Objects.Components
{
    public class GuiTextureComponent : MeshRendererComponent
    {
        public GuiTextureComponent(Texture texture): base(Mesh.GuiQuad, new Material(Shader.Gui, texture))
        {
        }

        public override void OnAwake()
        {
            this.vao = new GuiVertexArrayObject(this);
            this.Actor.RootScene.RenderSystem.AddVertexArrayObject(vao);
        }

        public override void OnShutdown()
        {
            this.Actor.RootScene.RenderSystem.RemoveVertexArrayObject(vao);
        }
    }
}
