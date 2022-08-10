using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImGuiNET;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using SharpGame;
using SharpGame.Core;
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
        public void OnDetach()
        {
            m_ImGuiController.Dispose();
        }

        public void OnAttach()
        {
            m_ImGuiController = new ImGuiController(1920, 1080);

            m_EditorScene = new Scene();
            m_ActiveScene = m_EditorScene;
        }

        public void OnUpdate(float deltaTime)
        {
            m_ImGuiController.Update(SharpGameWindow.Instance, deltaTime);
        }

        public void OnRender()
        {
            //GL.ClearColor(new Color4(0, 32, 48, 255));
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

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
            ImGui.DockSpace(ImGui.GetID("Dock"), Vector2.Zero, ImGuiDockNodeFlags.None);

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

            ImGui.Begin("Hierarchy");

            if (ImGui.BeginPopupContextWindow("ActorPopup"))
            {
                if (ImGui.MenuItem("Create Actor"))
                {
                    Actor actor = m_EditorScene.CreateActor();
                    m_SelectedActor = actor;
                }

                ImGui.EndPopup();
            }

            ImGui.End();

            ImGui.Begin("Viewport");
            ImGui.End();

            ImGui.Begin("Inspector");
            ImGui.End();

            ImGui.Begin("Assets");

            ImGui.End();

            ImGui.Begin("Information");
            ImGui.Text($"Actors: {m_EditorScene.ActorCount}");
            ImGui.End();
            ImGui.ShowDemoWindow();

            ImGui.End();


            m_ImGuiController.Render();
        }
    }
}
