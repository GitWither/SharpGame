using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Audio.OpenAL;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;

using SharpGame.Graphics;
using SharpGame.Graphics.Meshes;
using SharpGame.Objects;
using SharpGame.Util;

using BulletSharp;

namespace SharpGame
{
    public class SharpGameWindow : GameWindow
    {
        public static Scene ActiveScene;
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

            this.Title += " - " + GL.GetString(StringName.Renderer);
            this.VSync = VSyncMode.On;
            //this.TargetRenderFrequency = 60;

            base.UpdateFrame += this.UpdateFrameHandler;
            base.RenderFrame += this.RenderFrameHandler;
            base.Resize += this.ResizeHandler;
        }

        private void ResizeHandler(object sender, EventArgs e)
        {
            ActiveScene?.Camera.SetAspectRatio((float)this.Width / this.Height);
            GL.Viewport(this.ClientSize);
        }

        ~SharpGameWindow()
        {
            base.UpdateFrame -= this.UpdateFrameHandler;
            base.RenderFrame -= this.RenderFrameHandler;
            base.Resize -= this.ResizeHandler;
        }

        public void LoadScene(Scene scene)
        {
            if (ActiveScene != scene)
            {
                ActiveScene?.OnShutdown();
                ActiveScene = scene;
                ActiveScene.OnAwake();
            }
            else
            {
                Logger.Warn("This scene is already loaded. Skipping.");
            }
        }

        private void RenderFrameHandler(object sender, FrameEventArgs e)
        {
            GL.ClearColor(Color4.Black);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

            if (ActiveScene != null)
            {
                ActiveScene.Render();
            }
            else
            {
                Logger.Error("There isn't an active scene added. Can't draw anything!");
            }

            SwapBuffers();
        }


        private void UpdateFrameHandler(object sender, FrameEventArgs e)
        {
            ActiveScene?.OnUpdate((float)e.Time);
        }
    }
}
