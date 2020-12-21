using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;
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
        private static Scene activeScene;
        public static bool running;

        private Thread logic = new Thread(() =>
        {
            while (running)
            {
                //activeScene?.OnUpdate();
            }
        });

        public SharpGameWindow(int width, int height, string title) : base(width, height, GraphicsMode.Default, title, GameWindowFlags.Default)
        {
            if (Thread.CurrentThread.Name == null)
            {
                Thread.CurrentThread.Name = SharedConstants.MainThreadName;
            }


            base.UpdateFrame += this.UpdateFrameHandler;
            base.RenderFrame += this.RenderFrameHandler;
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
            //GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);

            GL.DepthFunc(DepthFunction.Less);

            GL.ClearColor(Color4.Black);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

            if (activeScene != null)
            {
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
            activeScene?.OnUpdate((float)e.Time);
        }
    }
}
