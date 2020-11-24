using OpenTK.Graphics.OpenGL4;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpGame.Objects.Components
{
    public class PlayerControlledComponent : Component
    {
        public override void OnUpdate()
        {
            base.OnUpdate();
            if (Input.GetKeyDown(Util.KeyCode.W))
            {
                this.Actor.PositionComponent.Translate(0.0f, 0.0f, 0.1f);
            }
            if (Input.GetKeyDown(Util.KeyCode.A))
            {
                this.Actor.PositionComponent.Translate(0.1f, 0.0f, 0);
            }
            if (Input.GetKeyDown(Util.KeyCode.S))
            {
                this.Actor.PositionComponent.Translate(0.0f, 0.0f, -0.1f);
            }
            if (Input.GetKeyDown(Util.KeyCode.D))
            {
                this.Actor.PositionComponent.Translate(-0.1f, 0.0f, 0.0f);
            }
            if (Input.GetKeyDown(Util.KeyCode.LeftShift))
            {
                this.Actor.PositionComponent.Translate(0.0f, -0.1f, 0.0f);
            }
            if (Input.GetKeyDown(Util.KeyCode.Space))
            {
                this.Actor.PositionComponent.Translate(0.0f, 0.1f, 0.0f);
            }
            if (Input.GetKeyDown(Util.KeyCode.Z))
            {
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            }
            if (Input.GetKeyUp(Util.KeyCode.Z))
            {
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            }
        }
    }
}
