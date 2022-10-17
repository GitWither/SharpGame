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
        public long MaterialAsset { get; set; }

        public MeshComponent(long meshAsset, long materialAsset)
        {
            this.MeshAsset = meshAsset;
            this.MaterialAsset = materialAsset;
        }
    }
}
