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
        public long MeshAsset { get; set; }
        public Material Material { get; set; }

        public MeshComponent(long meshAsset, Material material)
        {
            this.MeshAsset = meshAsset;
            this.Material = material;
        }
    }
}
