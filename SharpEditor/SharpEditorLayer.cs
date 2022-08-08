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
using SharpGame.Util;

namespace SharpEditor
{
    internal class SharpEditorLayer : ILayer
    {
        private ImGuiController m_ImGuiController;
        public void OnDetach()
        {
            m_ImGuiController.Dispose();
        }

        public void OnAttach()
        {
            m_ImGuiController = new ImGuiController(1920, 1080);
        }

        public void OnUpdate(float deltaTime)
        {
            m_ImGuiController.Update(SharpGameWindow.Instance, deltaTime);
        }

        public void OnRender()
        {
            GL.ClearColor(new Color4(0, 32, 48, 255));
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

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
            ImGui.End();

            ImGui.Begin("Viewport");
            ImGui.End();
            ImGui.ShowDemoWindow();


            m_ImGuiController.Render();
        }
    }
}
