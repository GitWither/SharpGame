using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;

using SharpGame.Graphics.Meshes;
using SharpGame.Objects;
using SharpGame.Util;

namespace SharpGame
{
    public class SharpGameWindow : GameWindow
    {
        private Scene activeScene;
        private Matrix4 projection;
        private Matrix4 view;

        public SharpGameWindow(int width, int height, string title) : base(width, height, GraphicsMode.Default, title)
        {
            base.UpdateFrame += this.UpdateFrameHandler;
            base.RenderFrame += this.RenderFrameHandler;

            this.projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver2, 16f / 9, 0.01f, 100f);
            this.view = Matrix4.Identity;
        }

        ~SharpGameWindow()
        {
            base.UpdateFrame -= this.UpdateFrameHandler;
            base.RenderFrame -= this.RenderFrameHandler;
        }

        public void LoadScene(Scene scene)
        {
            if (activeScene != scene)
            {
                activeScene?.OnShutdown();
                activeScene = scene;
                activeScene.OnAwake();
            }
        }

        private void RenderFrameHandler(object sender, FrameEventArgs e)
        {
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.LineSmooth);
            GL.Enable(EnableCap.PolygonSmooth);

            GL.DepthFunc(DepthFunction.Less);

            GL.ClearColor(Color4.Bisque);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

            if (activeScene != null)
            {
                Matrix4 mvp = view * projection;
                GL.UniformMatrix4(0, false, ref mvp);

                activeScene.BindShader();
                activeScene.BindTexture();

                activeScene.Render();

            }
            else
            {
                Logger.Error("There isn't an active scene added. Can't draw anything!");
            }

            SwapBuffers();
        }


        private void UpdateFrameHandler(object sender, FrameEventArgs e)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            Vector3 movementDelta = Vector3.Zero;


            if (keyboardState.IsKeyDown(Key.D))
            {
                movementDelta.X += 1;
            }
            if (keyboardState.IsKeyDown(Key.A))
            {
                movementDelta.X -= 1;
            }
            if (keyboardState.IsKeyDown(Key.Space))
            {
                movementDelta.Y += 1;
            }
            if (keyboardState.IsKeyDown(Key.ShiftLeft))
            {
                movementDelta.Y -= 1;
            }
            if (keyboardState.IsKeyDown(Key.S))
            {
                movementDelta.Z += 1;
            }
            if (keyboardState.IsKeyDown(Key.W))
            {
                movementDelta.Z -= 1;
            }
            if (keyboardState.IsKeyDown(Key.Z))
            {
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            }
            if (keyboardState.IsKeyUp(Key.Z))
            {
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            }

            if (movementDelta.LengthSquared > 0.0001f) 
            {
                view *= Matrix4.CreateTranslation(-movementDelta.Normalized() * 0.1f);
                Console.WriteLine(view);
            }

            activeScene?.OnUpdate();
        }
    }
}
