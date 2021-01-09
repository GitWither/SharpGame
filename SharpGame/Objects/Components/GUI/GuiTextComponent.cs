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
        public string Text
        {
            get => text;
            set
            {
                this.text = value;
                this.Mesh = Mesh.FromText(value);
                this.Rebuffer = true;
            }
        }
        internal bool Rebuffer { get; set; }
        private string text;
        public GuiTextComponent(string text, Texture font) : base(Mesh.FromText(text), new Material(Shader.Text, font))
        {
            this.text = text;
        }
    }
}
