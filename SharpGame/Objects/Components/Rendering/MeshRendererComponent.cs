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
        public Mesh Mesh { get; set; }
        public Material Material { get; set; }
        public bool Static { get; set; }

        protected VertexArrayObject vao;

        public MeshRendererComponent(Mesh mesh, Material material)
        {
            this.Mesh = mesh;
            this.Material = material;
        }

        public override void OnAwake()
        {
            this.vao = new MeshVertexArrayObject(this);
            this.Actor.RootScene.RenderSystem.AddWorldVertexArrayObject(vao);
        }

        public override void OnShutdown()
        {
            this.Actor.RootScene.RenderSystem.RemoveWorldVertexArrayObject(vao);
        }
    }
}
