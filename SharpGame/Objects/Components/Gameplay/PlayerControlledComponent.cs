using OpenTK.Graphics.OpenGL4;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpGame.Input;
using OpenTK;
using SharpGame.Util;

namespace SharpGame.Objects.Components
{
    public class PlayerControlledComponent : Component
    {
        public float factor = 15f;
        Vector3 Right = Vector3.UnitX;
        Vector3 Forward = Vector3.UnitZ;

        public override void OnUpdate(float deltaTime)
        {
            float forwardX = (float)(Math.Sin(MathHelper.DegreesToRadians(this.Actor.RotationComponent.Yaw)) * Math.Cos(MathHelper.DegreesToRadians(this.Actor.RotationComponent.Pitch)));
            float forwardY = (float)Math.Sin(MathHelper.DegreesToRadians(this.Actor.RotationComponent.Pitch));
            float forwardZ = (float)(Math.Cos(MathHelper.DegreesToRadians(this.Actor.RotationComponent.Yaw)) * Math.Cos(MathHelper.DegreesToRadians(this.Actor.RotationComponent.Pitch)));

            Forward = new Vector3(forwardX, forwardY, forwardZ);
            Right = Vector3.Cross(Vector3.UnitY, Forward);
            Right.NormalizeFast();

            Vector3 delta = Vector3.Zero;

            if (InputSystem.GetKeyDown(KeyCode.W))
            {
                delta.Z += factor;
            }
            if (InputSystem.GetKeyDown(KeyCode.A))
            {
                delta.X += factor;
            }
            if (InputSystem.GetKeyDown(KeyCode.S))
            {
                delta.Z -= factor;
            }
            if (InputSystem.GetKeyDown(KeyCode.D))
            {
                delta.X -= factor;
            }
            if (InputSystem.GetKeyDown(KeyCode.LeftShift))
            {
                delta.Y -= factor;
            }
            if (InputSystem.GetKeyDown(KeyCode.Space))
            {
                delta.Y += factor;
            }
            if (InputSystem.GetKeyDown(KeyCode.Z))
            {
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            }
            if (InputSystem.GetKeyUp(KeyCode.Z))
            {
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            }

            if (delta.LengthSquared > 0.0001f)
            {
                this.Actor.PositionComponent.Translate(((Right.X * delta.X) + (Forward.X * delta.Z))*deltaTime, delta.Y * deltaTime, ((Forward.Z * delta.Z) + (Right.Z * delta.X))*deltaTime);
            }

            Vector2 mouseDelta = InputSystem.GetMouseAxis() * 0.05f;
            this.Actor.RotationComponent.Rotate(-mouseDelta.Y, mouseDelta.X, 0);

            if (this.Actor.RotationComponent.Pitch >= 89)
            {
                this.Actor.RotationComponent.Set(89, this.Actor.RotationComponent.Yaw, this.Actor.RotationComponent.Roll);
            }
            else if (this.Actor.RotationComponent.Pitch <= -89)
            {
                this.Actor.RotationComponent.Set(-89, this.Actor.RotationComponent.Yaw, this.Actor.RotationComponent.Roll);
            }
        }
    }
}
