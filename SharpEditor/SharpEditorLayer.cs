using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImGuiNET;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using SharpEditor.Util;
using SharpGame;
using SharpGame.Core;
using SharpGame.Graphics;
using SharpGame.Objects;
using SharpGame.Util;
using Vector2 = System.Numerics.Vector2;

namespace SharpEditor
{
    internal class SharpEditorLayer : ILayer
    {
        private ImGuiController m_ImGuiController;

        private Scene m_EditorScene;
        private Scene m_ActiveScene;

        private Actor m_SelectedActor;

        private Framebuffer m_SceneBuffer; 
        public void OnDetach()
        {
            m_ImGuiController.Dispose();
        }

        public void OnAttach()
        {
            m_ImGuiController = new ImGuiController(1920, 1080);

            m_SceneBuffer = new Framebuffer(1920, 1080, 1);

            m_EditorScene = new Scene();
            m_ActiveScene = m_EditorScene;
        }

        public void OnUpdate(float deltaTime)
        {
            m_ImGuiController.Update(SharpGameWindow.Instance, deltaTime);
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

            if (ImGui.BeginMenuBar())
            {
                if (ImGui.BeginMenu("File"))
                {
                    if (ImGui.MenuItem("Save"))
                    {
                        Logger.Info("hello");
                    }
                    ImGui.EndMenu();
                }

                ImGui.EndMenuBar();
            }


            using (new ScopedMenu("Hierarchy"))
            {
                if (ImGui.BeginPopupContextWindow("ActorPopup"))
                {
                    if (ImGui.MenuItem("Create Actor"))
                    {
                        Actor actor = m_EditorScene.CreateActor();
                        m_SelectedActor = actor;
                    }

                    ImGui.EndPopup();
                }
            }

            using (new ScopedStyle(ImGuiStyleVar.WindowPadding, Vector2.Zero))
            {
                using (new ScopedMenu("Viewport"))
                {
                    Vector2 sizeAvail = ImGui.GetContentRegionAvail();
                    ImGui.Image((IntPtr)m_SceneBuffer.ColorAttachment, new Vector2(sizeAvail.X, sizeAvail.Y));
                }
            }

            using (new ScopedMenu("Inspector"))
            {

            }

            using (new ScopedMenu("Assets"))
            {

            }

            using (new ScopedMenu("Information"))
            {
                ImGui.Text($"Actors: {m_EditorScene.ActorCount}");
            }

            ImGui.ShowDemoWindow();

            ImGui.End();


            m_ImGuiController.Render();
        }

        public void OnRender()
        {
            m_SceneBuffer.Bind();
            GL.ClearColor(Color4.DarkSalmon);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);
            m_SceneBuffer.Unbind();

            OnImGuiRender();

        }
    }
}
