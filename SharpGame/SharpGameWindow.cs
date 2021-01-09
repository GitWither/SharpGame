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
using System.Runtime.CompilerServices;
using OpenTK.Audio;

namespace SharpGame
{
    public class SharpGameWindow : GameWindow
    {
        public static Scene ActiveScene;
        public static bool IsRunning;

        private readonly Thread logic = new Thread(UpdateThread);

        private ContextHandle audioContext;
        private IntPtr audioDevice;

        public SharpGameWindow(int width, int height, string title) : base(width, height, GraphicsMode.Default, title, GameWindowFlags.Default)
        {
            if (Thread.CurrentThread.Name == null)
            {
                Thread.CurrentThread.Name = SharedConstants.RenderThreadName;
            }

            audioDevice = Alc.OpenDevice(null);
            audioContext = Alc.CreateContext(audioDevice, (int[])null);
            Alc.MakeContextCurrent(audioContext);
            AL.DistanceModel(ALDistanceModel.LinearDistance);

            this.VSync = VSyncMode.On;

            base.UpdateFrame += this.UpdateFrameHandler;
            base.RenderFrame += this.RenderFrameHandler;
            base.Resize += this.ResizeHandler;

            IsRunning = true;
            //logic.Start();
        }

        private void ResizeHandler(object sender, EventArgs e)
        {
            ActiveScene?.Camera.SetAspectRatio((float)this.Width / this.Height);
            GL.Viewport(this.ClientSize);
        }

        ~SharpGameWindow()
        {
            if (audioContext != ContextHandle.Zero)
            {
                Alc.MakeContextCurrent(ContextHandle.Zero);
                Alc.DestroyContext(audioContext);
            }
            audioContext = ContextHandle.Zero;

            if (audioDevice != IntPtr.Zero)
            {
                Alc.CloseDevice(audioDevice);
            }
            audioDevice = IntPtr.Zero;

            base.UpdateFrame -= this.UpdateFrameHandler;
            base.RenderFrame -= this.RenderFrameHandler;
            base.Resize -= this.ResizeHandler;

            logic.Abort();
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

        private static void UpdateThread()
        {
            if (Thread.CurrentThread.Name == null)
            {
                Thread.CurrentThread.Name = SharedConstants.MainThreadName;
            }

            while (IsRunning)
            {
                ActiveScene?.OnUpdate(0.00001f);
            }
        }
    }
}
