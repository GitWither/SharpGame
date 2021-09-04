﻿using SharpGame.Graphics;
using SharpGame.Graphics.Meshes;
using SharpGame.Graphics.Vaos;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpGame.Objects.Components
{
    public class ParticleEmitterComponent : MeshRendererComponent
    {
        public int Count { get; set; }
        public float Velocity { get; set; }
        public float MaxLifetime { get; set; }

        //Mesh.GuiQuad is actually unused here. Perhaps I could add a way to add custom meshes to particles?
        public ParticleEmitterComponent(int count, float velocity, float life, Material material) : base(Mesh.GuiQuad, material)
        {
            this.Count = count;
            this.Velocity = velocity;
            this.MaxLifetime = life;
        }

        public override void OnAwake()
        {
            this.vao = new ParticleVertexArrayObject(this);
            this.vao.Upload();
        }

        public override void OnShutdown()
        {
            this.vao.Dispose();
        }
    }
}
