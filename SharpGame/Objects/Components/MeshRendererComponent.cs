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
        public bool Static { get; private set; }

        public MeshRendererComponent(Mesh mesh)
        {
            this.Mesh = mesh;
        }
    }
}
