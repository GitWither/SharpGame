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
using SharpGame.Graphics.Meshes;
using SharpGame.Objects;
using SharpGame.Objects.Components;
using SharpGame.Objects.Components.Transform;
using SharpGame.Util;
using Vector2 = System.Numerics.Vector2;

namespace SharpEditor
{
    internal class SharpEditorLayer : ILayer
    {
        private ImGuiController m_ImGuiController;

        private EditorCamera m_EditorCamera;

        private Scene m_EditorScene;
        private Scene m_ActiveScene;

        private Actor m_SelectedActor;

        private Framebuffer m_SceneBuffer;

        private Vector2 m_Viewport;
        public void OnDetach()
        {
            m_ImGuiController.Dispose();
        }

        public void OnAttach()
        {
            m_ImGuiController = new ImGuiController(1280, 720);

            m_SceneBuffer = new Framebuffer(1280, 720, 1);

            m_EditorCamera = new EditorCamera(30, 16/ 9f, 0.01f, 1000f);

            SharpGameWindow.Instance.MouseWheel += this.MouseScrolledEvent;
            SharpGameWindow.Instance.Resize += this.Resize;
            SharpGameWindow.Instance.Maximized += this.Maximize; 

            m_EditorScene = new Scene();
            m_ActiveScene = m_EditorScene;

            Actor actor = m_EditorScene.CreateActor();
            actor.AddComponent(new MeshComponent(Mesh.FromOBJ("buffalo"),
                new Material(Shader.Unlit, new Texture("buffalo"))));
            actor.AddComponent(new TransformComponent(new Vector3(0, 0, 0), Vector3.Zero, Vector3.One * 5));
            m_SelectedActor = actor;
        }

        private void Maximize(OpenTK.Windowing.Common.MaximizedEventArgs obj)
        {
            m_ImGuiController.WindowResized(SharpGameWindow.Instance.Size.X, SharpGameWindow.Instance.Size.Y);
        }

        private void Resize(OpenTK.Windowing.Common.ResizeEventArgs obj)
        {
            GL.Viewport(0, 0, obj.Width, obj.Height);
            m_ImGuiController.WindowResized(obj.Width, obj.Height);
        }

        private void MouseScrolledEvent(OpenTK.Windowing.Common.MouseWheelEventArgs obj)
        {
            m_EditorCamera.OnMouseScroll(obj.OffsetY);
            m_ImGuiController.MouseScroll(obj.Offset);
        }

        public void OnUpdate(float deltaTime)
        {
            m_EditorCamera.OnUpdate(deltaTime);
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

            using (new ScopedStyle(ImGuiStyleVar.WindowPadding, Vector2.Zero))
            {
                using (new ScopedMenu("Viewport"))
                {
                    Vector2 sizeAvail = ImGui.GetContentRegionAvail();
                    m_Viewport = new Vector2(sizeAvail.X, sizeAvail.Y);
                    ImGui.Image((IntPtr)m_SceneBuffer.ColorAttachment, m_Viewport, new Vector2(0, 1), new Vector2(1, 0));
                }
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


            using (new ScopedMenu("Inspector"))
            {
                ref TransformComponent transform = ref m_SelectedActor.GetComponent<TransformComponent>();
                System.Numerics.Vector3 vector = new System.Numerics.Vector3(transform.Position.X, transform.Position.Y,
                    transform.Position.Z);
                ImGui.DragFloat3("Pos", ref vector);
                transform.Position = new Vector3(vector.X, vector.Y, vector.Z);
            }

            using (new ScopedMenu("Assets"))
            {
                ImGui.Columns(5, "cool");
                foreach (string path in Directory.EnumerateFileSystemEntries("."))
                {
                    ImGui.Button(path);

                    ImGui.NextColumn();
                }
            }

            using (new ScopedMenu("Information"))
            {
                ImGui.Text($"Actors: {m_EditorScene.ActorCount}");
                m_EditorCamera.Orientation.ToEulerAngles(out Vector3 angles);
                ImGui.Text($"Camera Pos: {angles}");
                ImGui.Text($"Buffalo Pos: {m_SelectedActor.GetComponent<TransformComponent>().Position}");
            }

            ImGui.ShowDemoWindow();

            ImGui.End();


            m_ImGuiController.Render();
        }

        public void OnRender()
        {
            if (m_Viewport.X > 0.0f && m_Viewport.Y > 0.0f && (m_SceneBuffer.Width != m_Viewport.X || m_SceneBuffer.Height != m_Viewport.Y))
            {
                m_SceneBuffer.Resize((int)m_Viewport.X, (int)m_Viewport.Y);
                m_EditorCamera.UpdateViewportSize(m_Viewport.X, m_Viewport.Y);
            }

            m_SceneBuffer.Bind();
            GL.ClearColor(Color4.DarkSalmon);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);
            //GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);
            GL.Disable(EnableCap.CullFace);

            m_EditorScene.OnRender(m_EditorCamera);
            
            m_SceneBuffer.Unbind();

            OnImGuiRender();
        }
    }
}
