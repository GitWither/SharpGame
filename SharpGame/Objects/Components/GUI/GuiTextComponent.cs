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
    public class GuiTextComponent : MeshRendererComponent
    {
        public string Text
        {
            get => text;
            set
            {
                this.text = value;
                this.Mesh = Mesh.FromText(value);
            }
        }
        private string text;
        public GuiTextComponent(string text, Texture font) : base(Mesh.FromText(text), new Material(Shader.Text, font))
        {
            this.text = text;
        }

        public override void OnAwake()
        {
            this.vao = new TextVertexArrayObject(this);
            this.Actor.RootScene.RenderSystem.AddVertexArrayObject(vao);
        }

        public override void OnShutdown()
        {
            this.Actor.RootScene.RenderSystem.RemoveVertexArrayObject(vao);
        }
    }
}
