using OpenTK.Audio.OpenAL;

using SharpGame.Graphics;
using SharpGame.Graphics.Meshes;
using SharpGame.Graphics.Vaos;
using SharpGame.Util;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpGame.Objects.Components
{
    public struct MeshComponent
    {
        public Mesh Mesh { get; set; }
        public Material Material { get; set; }

        public MeshComponent(Mesh mesh, Material material)
        {
            this.Mesh = mesh;
            this.Material = material;
        }
    }
}
