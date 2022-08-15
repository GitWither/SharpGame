using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
using Vector4 = System.Numerics.Vector4;

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
        private bool m_OnViewport;

        private Color4 m_BackgroundColor;
        private string m_CurrentDirectorty = Directory.GetCurrentDirectory();
        public void OnDetach()
        {
            m_ImGuiController.Dispose();
        }

        public void OnAttach()
        {
            m_ImGuiController = new ImGuiController(1280, 720);

            m_SceneBuffer = new Framebuffer(1280, 720, 1);

            m_EditorCamera = new EditorCamera(30.0f, 16/ 9f, 0.01f, 1000f, 1280, 720);

            SharpGameWindow.Instance.MouseWheel += this.MouseScrolledEvent;
            SharpGameWindow.Instance.Resize += this.Resize;
            SharpGameWindow.Instance.Maximized += this.Maximize;
            SharpGameWindow.Instance.TextInput += this.TextInput;
            

            m_EditorScene = new Scene();
            m_ActiveScene = m_EditorScene;

            Actor actor = m_EditorScene.CreateActor();
            actor.AddComponent(new MeshComponent(Mesh.FromOBJ("terrain"),
                new Material(Shader.Unlit, new Texture("grass20"))));
            actor.AddComponent(new TransformComponent(new Vector3(0, 0, 0), Vector3.Zero, Vector3.One * 0.1f));
            m_SelectedActor = actor;
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
            m_EditorCamera.OnMouseScroll(obj.OffsetY);
            m_ImGuiController.MouseScroll(obj.Offset);
        }

        public void OnUpdate(float deltaTime)
        {
            if (m_OnViewport)
            {
                m_EditorCamera.OnUpdate(deltaTime);
            }

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
                using (new ScopedMenu("Game View"))
                {
                    m_OnViewport = ImGui.IsWindowFocused() || ImGui.IsWindowHovered();

                    Vector2 sizeAvail = ImGui.GetContentRegionAvail();
                    m_Viewport = new Vector2(sizeAvail.X, sizeAvail.Y);
                    ImGui.Image((IntPtr)m_SceneBuffer.ColorAttachment, m_Viewport, new Vector2(0, 1), new Vector2(1, 0));
                }
            }

            using (new ScopedMenu("Actors"))
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
                ImGui.DragFloat3("Pos", ref vector, 0.05f);
                transform.Position = new Vector3(vector.X, vector.Y, vector.Z);

                System.Numerics.Vector3 vector2 = new System.Numerics.Vector3(transform.Rotation.X, transform.Rotation.Y,
                    transform.Rotation.Z);
                ImGui.DragFloat3("Rot", ref vector2, 0.05f);
                transform.Rotation = new Vector3(vector2.X, vector2.Y, vector2.Z);

                System.Numerics.Vector3 vector3 = new System.Numerics.Vector3(transform.Scale.X, transform.Scale.Y,
                    transform.Scale.Z);
                ImGui.DragFloat3("Scale", ref vector3, 0.05f);
                transform.Scale = new Vector3(vector3.X, vector3.Y, vector3.Z);
            }

            using (new ScopedMenu("Assets"))
            {
                if (ImGui.Button("<"))
                {
                    m_CurrentDirectorty = Directory.GetParent(m_CurrentDirectorty).FullName;
                }
                ImGui.Columns(5, "cool");
                foreach (string path in Directory.EnumerateFileSystemEntries(m_CurrentDirectorty))
                {
                    if (Directory.Exists(path))
                    {
                        if (ImGui.Button(Path.GetFileNameWithoutExtension(path)))
                        {
                            m_CurrentDirectorty = Path.Combine(m_CurrentDirectorty, path);
                        }
                    }
                    else
                    {
                        ImGui.Text(Path.GetFileName(path));
                    }

                    ImGui.NextColumn();
                }
            }

            using (new ScopedMenu("Information"))
            {
                ImGui.Text($"Actors: {m_EditorScene.ActorCount}");

                Vector4 colorRef = new Vector4(m_BackgroundColor.R, m_BackgroundColor.G, m_BackgroundColor.B,m_BackgroundColor.A);
                ImGui.ColorEdit4("Background Color", ref colorRef);
                m_BackgroundColor = new Color4(colorRef.X, colorRef.Y, colorRef.Z, colorRef.W);
            }

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
            GL.ClearColor(m_BackgroundColor);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);
            //GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);
            GL.Disable(EnableCap.CullFace);

            m_EditorScene.OnRender(m_EditorCamera);
            
            m_SceneBuffer.Unbind();

            //GL.Viewport(0, 0, this.);

            OnImGuiRender();
        }
    }
}
