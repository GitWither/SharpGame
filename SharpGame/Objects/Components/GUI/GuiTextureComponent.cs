using OpenTK.Graphics.OpenGL4;

using SharpGame.Graphics;
using SharpGame.Graphics.Meshes;

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
    }
}
