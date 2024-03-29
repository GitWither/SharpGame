﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ImGuiNET;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using SharpEditor.Panels;
using SharpEditor.Util;
using SharpGame;
using SharpGame.Assets;
using SharpGame.Core;
using SharpGame.Graphics;
using SharpGame.Graphics.Meshes;
using SharpGame.Objects;
using SharpGame.Objects.Components;
using SharpGame.Serialization;
using SharpGame.Util;
using TinyDialogsNet;
using Boolean = System.Boolean;
using Vector2 = System.Numerics.Vector2;
using Vector3 = System.Numerics.Vector3;
using Vector4 = System.Numerics.Vector4;

namespace SharpEditor
{
    internal class SharpEditorLayer : ILayer
    {
        private ImGuiController m_ImGuiController;

        private EditorCamera m_EditorCamera;

        private Scene m_EditorScene;
        private Scene m_ActiveScene;

        private Framebuffer m_SceneBuffer;

        private Vector2 m_Viewport;
        private bool m_OnViewport;

        private Vector3 m_BackgroundColor;

        private InspectorPanel m_Inspector;
        private ActorsPanel m_Actors;
        private AssetsPanel m_Assets;
        private MaterialPanel m_MaterialEditor;

        private int m_HoveredActor;
        private readonly Vector2[] m_ViewportBounds = new Vector2[2];

        private Project m_CurrentProject;

        private bool m_ProjectModalOpen = false;
        private bool m_MaterialWindowOpen = false;

        private readonly IntPtr m_CurrentProjectName = Marshal.StringToHGlobalAnsi(string.Empty);
        private readonly IntPtr m_CurrentProjectPath = Marshal.StringToHGlobalAnsi(string.Empty);

        private SceneStatus m_SceneStatus = SceneStatus.Editor;

        public void OnDetach()
        {
            Marshal.FreeHGlobal(m_CurrentProjectPath);
            Marshal.FreeHGlobal(m_CurrentProjectName);
            m_ImGuiController.Dispose();
        }

        public void OnAttach()
        {
            m_ImGuiController = new ImGuiController(1280, 720);


            FramebufferInfo info = new FramebufferInfo
            {
                Width = 1280,
                Height = 720,
                Samples = 1,
                ColorAttachments = new FramebufferAttachmentInfo[] { FramebufferTextureFormat.Rgba8},
                DepthAttachment = FramebufferTextureFormat.Depth24Stencil8
            };
            m_SceneBuffer = new Framebuffer(info);


            m_Actors = new ActorsPanel();
            m_Inspector = new InspectorPanel(m_Actors);
            m_Assets = new AssetsPanel();
            m_MaterialEditor = new MaterialPanel();

            m_Actors.SelectedActor = Actor.Null;


            m_EditorCamera = new EditorCamera(30.0f, 16/ 9f, 0.01f, 1000f, 1280, 720);

            SharpGameWindow.Instance.MouseWheel += this.MouseScrolledEvent;
            SharpGameWindow.Instance.Resize += this.Resize;
            SharpGameWindow.Instance.Maximized += this.Maximize;
            SharpGameWindow.Instance.TextInput += this.TextInput;
            SharpGameWindow.Instance.FileDrop += this.FileDrop;
            

            m_EditorScene = new Scene();
            m_ActiveScene = m_EditorScene;

            m_Actors.Scene = m_EditorScene;

            /*
            Actor actor = m_EditorScene.CreateActor("Terrain");
            actor.AddComponent(new MeshComponent(Mesh.FromOBJ("terrain"),
                new Material(Shader.Unlit, new Texture("grass20"))));
            m_Actors.SelectedActor = actor;

            Actor buffalo = m_EditorScene.CreateActor("Buffalo");
            buffalo.AddComponent(new MeshComponent(Mesh.FromOBJ("buffalo"),
                new Material(Shader.Unlit, new Texture("buffalo"))));
            */

            //SceneSerializer.Serialize(m_EditorScene, "coolScene.json");
            SceneSerializer.Deserialize("coolScene.json", ref m_EditorScene);
        }

        private void FileDrop(FileDropEventArgs obj)
        {
            foreach (string file in obj.FileNames)
            {
                SharpGameWindow.Instance.AssetStorage.ImportAsset(file);
            }
        }

        private void TextInput(OpenTK.Windowing.Common.TextInputEventArgs obj)
        {
            m_ImGuiController.PressChar((char)obj.Unicode);
        }

        private void Maximize(OpenTK.Windowing.Common.MaximizedEventArgs obj)
        {
            m_ImGuiController.WindowResized(SharpGameWindow.Instance.Size.X, SharpGameWindow.Instance.Size.Y);
        }

        private void Resize(OpenTK.Windowing.Common.ResizeEventArgs obj)
        {
            m_ImGuiController.WindowResized(obj.Width, obj.Height);
        }

        private void MouseScrolledEvent(OpenTK.Windowing.Common.MouseWheelEventArgs obj)
        {
            if (m_OnViewport)
            {
                m_EditorCamera.OnMouseScroll(obj.OffsetY);
            }
            m_ImGuiController.MouseScroll(obj.Offset);
        }

        public void OnUpdate(float deltaTime)
        {
            if (m_OnViewport)
            {
                m_EditorCamera.OnUpdate(deltaTime);
            }

            m_ImGuiController.Update(SharpGameWindow.Instance, deltaTime);

            m_ActiveScene.OnUpdate(deltaTime);
        }

        private void OnImGuiRender()
        {
            ImGuiViewportPtr viewport = ImGui.GetMainViewport();
            ImGui.SetNextWindowPos(viewport.Pos);
            ImGui.SetNextWindowSize(viewport.Size);
            ImGui.SetNextWindowViewport(viewport.ID);

            ImGui.PushStyleVar(ImGuiStyleVar.WindowRounding, 0.0f);
            ImGui.PushStyleVar(ImGuiStyleVar.WindowBorderSize, 0.0f);

            ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, Vector2.Zero);
            ImGui.Begin("Dockspace",
                ImGuiWindowFlags.MenuBar |
                ImGuiWindowFlags.NoDocking |
                ImGuiWindowFlags.NoBackground |
                ImGuiWindowFlags.NoTitleBar |
                ImGuiWindowFlags.NoCollapse |
                ImGuiWindowFlags.NoResize |
                ImGuiWindowFlags.NoMove |
                ImGuiWindowFlags.NoBringToFrontOnFocus |
                ImGuiWindowFlags.NoNavFocus
                );

            ImGuiIOPtr io = ImGui.GetIO();
            ImGuiStylePtr style = ImGui.GetStyle();
            float minWinSizeX = style.WindowMinSize.X;
            style.WindowMinSize.X = 370.0f;
            ImGui.DockSpace(ImGui.GetID("Dock"), Vector2.Zero, ImGuiDockNodeFlags.PassthruCentralNode);

            style.WindowMinSize.X = minWinSizeX;

            ImGui.PopStyleVar(3);

            if (ImGui.BeginPopupModal("Project Config"))
            {
                ImGui.InputText("Project Name", m_CurrentProjectName, 24);
                string project = Marshal.PtrToStringAnsi(m_CurrentProjectName, 24);

                ImGui.InputText("Path", m_CurrentProjectPath, 256);
                string projectPath = Marshal.PtrToStringAnsi(m_CurrentProjectPath, 256);

                ImGui.EndPopup();
            }

            if (m_ProjectModalOpen)
            {
                ImGui.OpenPopup("Project Config");
                m_ProjectModalOpen = false;
            }

            if (ImGui.BeginMenuBar())
            {
                if (ImGui.BeginMenu("File"))
                {
                    if (ImGui.MenuItem("New Project"))
                    {
                        m_ProjectModalOpen = true;
                    }

                    if (ImGui.MenuItem("Open Project"))
                    {

                    }

                    ImGui.Separator();
                    if (ImGui.MenuItem("Open"))
                    {
                        string[] files = (string[])Dialogs.OpenFileDialog("Select Scene", Directory.GetCurrentDirectory(), new[] { "*.json" }, "SharpGame Scene", false);
                        SceneSerializer.Deserialize(files[0], ref m_EditorScene);
                    }
                    if (ImGui.MenuItem("Save"))
                    {
                        string file = Dialogs.SaveFileDialog("Save SharpGame Scene", Directory.GetCurrentDirectory(), "*.json", "SharpGame Scene (*.json)");
                        SceneSerializer.Serialize(m_EditorScene, file);
                    }

                    ImGui.Separator();

                    if (ImGui.MenuItem("Reload Behavior Assembly"))
                    {
                        SharpGameWindow.Instance.BehaviorManager.ReloadAssembly();
                    }


                    ImGui.EndMenu();
                }

                if (ImGui.BeginMenu("Window"))
                {
                    if (ImGui.MenuItem("Material Editor", "", m_MaterialWindowOpen))
                    {
                        m_MaterialWindowOpen = !m_MaterialWindowOpen;
                    }

                    ImGui.EndMenu();
                }

                ImGui.EndMenuBar();
            }

            using (new ScopedStyle(ImGuiStyleVar.WindowPadding, Vector2.Zero))
            {
                using (new ScopedMenu("Game View"))
                {
                    if (ImGui.Button(m_SceneStatus == SceneStatus.Editor ? "Play" : "Stop"))
                    {
                        m_SceneStatus = m_SceneStatus == SceneStatus.Playing ? SceneStatus.Editor : SceneStatus.Playing;

                        if (m_SceneStatus == SceneStatus.Playing)
                        {
                            m_ActiveScene = new Scene();
                            m_ActiveScene.CopyContentFrom(m_EditorScene);

                            m_Actors.Scene = m_ActiveScene;

                            m_ActiveScene.OnAwake();

                            Logger.Info("scene started");
                        }
                        else
                        {
                            Logger.Info("scene stopped");
                            m_ActiveScene.OnSleep();
                            m_ActiveScene = m_EditorScene;
                            m_Actors.Scene = m_ActiveScene;
                        }
                    }

                    m_OnViewport = ImGui.IsWindowFocused() || ImGui.IsWindowHovered();

                    Vector2 sizeAvail = ImGui.GetContentRegionAvail();
                    m_Viewport = new Vector2(sizeAvail.X, sizeAvail.Y);
                    ImGui.Image((IntPtr)m_SceneBuffer.GetColorAttachment(), m_Viewport, new Vector2(0, 1), new Vector2(1, 0));

                    Vector2 gameViewMinRegion = ImGui.GetWindowContentRegionMin();
                    Vector2 gameViewMaxRegion = ImGui.GetWindowContentRegionMax();
                    Vector2 gameViewOffset = ImGui.GetWindowPos();

                    m_ViewportBounds[0] = new Vector2(gameViewMinRegion.X + gameViewOffset.X, gameViewMinRegion.Y + gameViewOffset.Y);
                    m_ViewportBounds[1] = new Vector2(gameViewMaxRegion.X + gameViewOffset.X, gameViewMaxRegion.Y + gameViewOffset.Y);
                }
            }

            //TODO: Make this customizable probably
            bool shouldShow = true;
            m_Inspector.OnImGuiRender(ref shouldShow);
            m_Actors.OnImGuiRender(ref shouldShow);
            m_Assets.OnImGuiRender(ref shouldShow);


            using (new ScopedMenu("Information"))
            {
                ImGui.Text($"Actors: {m_ActiveScene.ActorCount}");

                ImGui.Text($"Hovered Actor: {m_HoveredActor}");

                ImGui.Text($"Behavior Instances: {SharpGameWindow.Instance.BehaviorManager.BehaviorInstances}");

                ImGui.ColorEdit3("Background Color", ref m_BackgroundColor);

                if (m_Actors.SelectedActor != Actor.Null)
                {
                    ImGui.Text($"Selected Actor: {m_Actors.SelectedActor.GetComponent<NameComponent>().Name}");
                }
            }

            using (new ScopedMenu("Renderer"))
            {
                ImGui.Text($"Draw Calls: {SharpGameWindow.Instance.Renderer.DrawCalls}");
                ImGui.Text($"Vertices: {SharpGameWindow.Instance.Renderer.Vertices}");
                ImGui.Text($"Indices: {SharpGameWindow.Instance.Renderer.Indices}");
            }

            using (new ScopedMenu("Asset Storage"))
            {

                foreach (var idAssetPair in SharpGameWindow.Instance.AssetStorage.EnumerateAssets())
                {
                    ImGui.Text("Id:");
                    ImGui.SameLine();
                    ImGui.Text(idAssetPair.Key.ToString());

                    ImGui.Text("Type:");
                    ImGui.SameLine();
                    ImGui.Text(idAssetPair.Value.Type.ToString());

                    ImGui.Text("Path:");
                    ImGui.SameLine();
                    ImGui.Text(Path.GetFileName(idAssetPair.Value.Path));

                    if (idAssetPair.Value.Type == AssetType.Material)
                    {
                        using ScopedId id = new ScopedId(idAssetPair.Key.ToString());

                        if (ImGui.Button("Select"))
                        {
                            m_MaterialEditor.ContextMaterial =
                                SharpGameWindow.Instance.AssetStorage.GetAsset<Material>(idAssetPair.Key);
                        }
                    }

                    ImGui.Separator();
                }
            }

            if (m_MaterialWindowOpen)
            {
                m_MaterialEditor.OnImGuiRender(ref m_MaterialWindowOpen);
            }

            ImGui.End();


            m_ImGuiController.Render();
        }

        private void CreateNewProject(string path)
        {

        }

        public void OnRender()
        {
            if (m_Viewport.X > 0.0f && m_Viewport.Y > 0.0f && (m_SceneBuffer.Width != m_Viewport.X || m_SceneBuffer.Height != m_Viewport.Y))
            {
                m_SceneBuffer.Resize((int)m_Viewport.X, (int)m_Viewport.Y);
                m_EditorCamera.UpdateViewportSize(m_Viewport.X, m_Viewport.Y);
            }

            m_SceneBuffer.Bind();

            RenderCommand.SetColor(m_BackgroundColor);

            m_ActiveScene.OnRender(m_EditorCamera);

            m_SceneBuffer.Unbind();


            OnImGuiRender();
        }
    }
}
