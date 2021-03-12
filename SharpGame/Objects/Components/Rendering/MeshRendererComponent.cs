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
    public class MeshRendererComponent : Component
    {
        public Mesh Mesh
        {
            get => mesh;
            set
            {
                this.mesh = value;
                this.vao?.Upload();
            }
        }
        public Material Material { get; set; }
        public bool Static { get; set; }

        protected VertexArrayObject vao;
        private Mesh mesh;

        public MeshRendererComponent(Mesh mesh, Material material)
        {
            this.mesh = mesh;
            this.Material = material;
        }

        public override void OnAwake()
        {
            this.vao = new MeshVertexArrayObject(this);
            this.Actor.RootScene.RenderSystem.AddVertexArrayObject(vao);
        }

        public override void OnShutdown()
        {
            this.Actor.RootScene.RenderSystem.RemoveVertexArrayObject(vao);
            Material.Dispose();
        }
    }
}
