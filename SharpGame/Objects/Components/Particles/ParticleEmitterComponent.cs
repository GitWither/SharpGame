using SharpGame.Graphics;
using SharpGame.Graphics.Meshes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpGame.Objects.Components
{
    class ParticleEmitterComponent : MeshRendererComponent
    {
        public int Count { get; set; }
        public float Velocity { get; set; }
        public float Life { get; set; }

        public ParticleEmitterComponent(int count, float velocity, float life, Material material) : base(Mesh.GuiQuad, material)
        {
            this.Count = count;
            this.Velocity = velocity;
            this.Life = life;
        }
    }
}
