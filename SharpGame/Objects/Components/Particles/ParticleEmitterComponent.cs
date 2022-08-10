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
    public struct ParticleEmitterComponent
    {
        public int Count { get; set; }
        public float Velocity { get; set; }
        public float MaxLifetime { get; set; }

        public Material Material { get; set; }

        public ParticleEmitterComponent(int count, float velocity, float maxLifetime, Material material)
        {
            this.Count = count;
            this.Velocity = velocity;
            this.MaxLifetime = maxLifetime;
            this.Material = material;
        }
    }
}
