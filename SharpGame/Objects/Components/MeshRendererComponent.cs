using OpenTK.Audio.OpenAL;

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
        public bool Dirty { get; set; }
        public PositionComponent LastPositionComponent { get; private set; }

        public MeshRendererComponent(Mesh mesh)
        {
            this.Mesh = mesh;
            Static = true;
            Dirty = false;
        }

        public override void OnAwake()
        {
            this.LastPositionComponent = this.Actor.PositionComponent;
        }

        public override void OnUpdate()
        {
            if (this.LastPositionComponent != null && !this.LastPositionComponent.Equals(this.Actor.PositionComponent))
            {
                this.LastPositionComponent = this.Actor.PositionComponent;
                Dirty = true;
            }
        }
    }
}
