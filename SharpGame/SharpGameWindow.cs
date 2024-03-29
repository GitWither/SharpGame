﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using SharpGame.Assets;
using SharpGame.Core;
using SharpGame.Scripting;

namespace SharpGame
{
    public class SharpGameWindow : GameWindow
    {
        private readonly Stack<ILayer> m_Layers;
        public BehaviorManager BehaviorManager { get; }
        public Renderer Renderer { get; }
        public AssetStorage AssetStorage { get; }
        public static SharpGameWindow Instance { get; private set; } = null;

        public SharpGameWindow(int width, int height, string title) : base(
            GameWindowSettings.Default,
            new NativeWindowSettings()
            {
                Size = new Vector2i(width, height),
                Title = title,
                NumberOfSamples = 3,
                APIVersion = new Version(4, 6)
            })
        {
            Thread.CurrentThread.Name = SharedConstants.RenderThreadName;

            AssetStorage = new AssetStorage();
            BehaviorManager = new BehaviorManager();
            Renderer = new Renderer();

            this.m_Layers = new Stack<ILayer>();
            Instance = this;

            AssetStorage.Initialize();
            BehaviorManager.Initialize();
            Renderer.Initialize();
        }

        protected override void OnLoad()
        {
            InputSystem.Init(this.KeyboardState, this.MouseState);

            ALBase.RegisterOpenALResolver();
            AL.DistanceModel(ALDistanceModel.LinearDistance);

            //this.VSync = VSyncMode.On;
            base.OnLoad();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            AssetStorage.Shutdown();
        }

        protected override void OnClosed()
        {
            foreach (ILayer layer in m_Layers)
            {
                layer.OnDetach();
            }
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            foreach (ILayer layer in m_Layers)
            {
                layer.OnRender();
            }
            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            foreach (ILayer layer in m_Layers)
            {
                layer.OnUpdate((float)args.Time);
            }
        }

        public void PushLayer(ILayer layer)
        {
            m_Layers.Push(layer);
            layer.OnAttach();
        }
    }
}
