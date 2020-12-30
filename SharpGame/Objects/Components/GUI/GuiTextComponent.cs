using SharpGame.Graphics;
using SharpGame.Graphics.Meshes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpGame.Objects.Components
{
    public class GuiTextComponent : MeshRendererComponent
    {
        public string Text { get; set; }
        public GuiTextComponent(string text, Texture font) : base(Mesh.FromText(text), new Material(Shader.Unlit, font))
        {
            this.Text = text;
        }
    }
}
