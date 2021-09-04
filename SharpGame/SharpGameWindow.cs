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

        private Thread updateThread;

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
            InputSystem.keyboardState = this.KeyboardState;
            InputSystem.mouseState = this.MouseState;

            ALBase.RegisterOpenALResolver();
            AL.DistanceModel(ALDistanceModel.LinearDistance);

            updateThread = new Thread(OnUpdate);
            updateThread.Start();

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
        }

        protected void OnUpdate()
        {
            Stopwatch sw = new Stopwatch();
            Thread.CurrentThread.Name = SharedConstants.LogicThreadName;

            while (ActiveScene.Running)
            {
                sw.Restart();

                this.Title = sw.Elapsed.TotalSeconds.ToString();
                ActiveScene.OnUpdate((float)sw.Elapsed.TotalSeconds);
            }


            ActiveScene.OnShutdown();

            return;
        }
    }
}
