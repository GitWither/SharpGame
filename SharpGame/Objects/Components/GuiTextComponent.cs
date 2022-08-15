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
    public struct GuiTextComponent
    {
        public string Text { get; set; }
        public Texture Font { get; set; }

        public GuiTextComponent(string text, Texture font)
        {
            this.Text = text;
            this.Font = font;
        }
    }
}
