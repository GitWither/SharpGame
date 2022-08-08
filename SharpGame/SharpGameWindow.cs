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
using SharpGame.Events;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Diagnostics;

namespace SharpGame
{
    public class SharpGameWindow : GameWindow
    {
        public static Scene ActiveScene;

        public SharpGameWindow(int width, int height, string title) : base(
            GameWindowSettings.Default,
            new NativeWindowSettings()
            {
                Size = new Vector2i(width, height),
                Title = title,
                NumberOfSamples = 3
            })
        {
            Thread.CurrentThread.Name = SharedConstants.RenderThreadName;
        }

        protected override void OnLoad()
        {
            InputSystem.Init(this.KeyboardState, this.MouseState);

            ALBase.RegisterOpenALResolver();
            AL.DistanceModel(ALDistanceModel.LinearDistance);

            //this.VSync = VSyncMode.On;
            base.OnLoad();
        }

        protected override void OnClosed()
        {
            ActiveScene.Running = false;
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            GL.Viewport(0, 0, e.Size.X, e.Size.Y);
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
            GL.BlendFunc(BlendingFactor.Zero, BlendingFactor.Zero);
            GL.DepthFunc(DepthFunction.Less);
            GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.DepthTest);
            GL.Disable(EnableCap.Blend);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

            if (ActiveScene != null)
            {
                Stopwatch sw = Stopwatch.StartNew();
                ActiveScene.OnRender();
                sw.Stop();
                this.Title = sw.ElapsedMilliseconds.ToString();
            }
            else
            {
                Logger.Error("There isn't an active scene added. Can't draw anything!");
            }

            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            ActiveScene.OnUpdate((float)args.Time);
        }
    }
}
