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
            activeScene?.OnUpdate();
        }
    }
}
