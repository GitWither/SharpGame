using OpenTK.Audio.OpenAL;

using SharpGame.Graphics;
using SharpGame.Graphics.Meshes;
using SharpGame.Util;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpGame.Objects.Components
{
    public class MeshRendererComponent : Component
    {
        public Mesh Mesh { get; private set; }
        public Texture Texture { get; private set; }
        public Shader Shader { get; private set; }
        public bool Static { get; private set; }

        public MeshRendererComponent(Mesh mesh, Texture texture, Shader shader)
        {
            this.Mesh = mesh;
            this.Texture = texture;
            this.Shader = shader;
        }
    }
}
