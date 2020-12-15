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
        public override void OnUpdate(float deltaTime)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                this.Actor.PositionComponent.Translate(0.0f, 0.0f, 5f * deltaTime);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                this.Actor.PositionComponent.Translate(5f * deltaTime, 0.0f, 0);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                this.Actor.PositionComponent.Translate(0.0f, 0.0f, -5f * deltaTime);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                this.Actor.PositionComponent.Translate(-5f * deltaTime, 0.0f, 0.0f);
            }
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                this.Actor.PositionComponent.Translate(0.0f, -5f * deltaTime, 0.0f);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                this.Actor.PositionComponent.Translate(0.0f, 5f * deltaTime, 0.0f);
            }
            if (Input.GetKeyDown(KeyCode.Z))
            {
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            }
            if (Input.GetKeyUp(KeyCode.Z))
            {
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            }
        }
    }
}
