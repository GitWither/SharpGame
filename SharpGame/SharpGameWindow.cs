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

using System.Runtime.CompilerServices;
using OpenTK.Audio;
using System.Runtime.InteropServices.ComTypes;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
using SharpGame.Input;

namespace SharpGame
{
    public class SharpGameWindow : GameWindow
    {
        public static Scene ActiveScene;


        public SharpGameWindow(int width, int height, string title) : base(
            GameWindowSettings.Default,
            new NativeWindowSettings()
            {
                Size = new OpenTK.Mathematics.Vector2i(width, height),
                Title = title
            })
        {
            if (Thread.CurrentThread.Name == null)
            {
                Thread.CurrentThread.Name = SharedConstants.RenderThreadName;
            }

            InputSystem.keyboardState = this.KeyboardState;
            InputSystem.mouseState = this.MouseState;

            ALBase.RegisterOpenALResolver();
            AL.DistanceModel(ALDistanceModel.LinearDistance);

            this.VSync = VSyncMode.On;
        }

        protected override void OnClosed()
        {
            ActiveScene.OnShutdown();

            base.OnClosed();
        }


        protected override void OnResize(ResizeEventArgs e)
        {
            GL.Viewport(0, 0, e.Size.X, e.Size.Y);
            ActiveScene?.Camera.SetAspectRatio(this.Size.X / (float)this.Size.Y);

            base.OnResize(e);
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

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

            if (ActiveScene != null)
            {
                ActiveScene.OnRender();
            }
            else
            {
                Logger.Error("There isn't an active scene added. Can't draw anything!");
            }

            SwapBuffers();

            base.OnRenderFrame(e);
        }


        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            ActiveScene?.OnUpdate((float)e.Time);

            base.OnUpdateFrame(e);
        }
    }
}
