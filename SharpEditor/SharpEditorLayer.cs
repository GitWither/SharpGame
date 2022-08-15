using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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
using SharpGame.Util;
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

            Actor actor = m_EditorScene.CreateActor("Terrain");
            actor.AddComponent(new MeshComponent(Mesh.FromOBJ("terrain"),
                new Material(Shader.Unlit, new Texture("grass20"))));
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
                if (ImGui.Button("Create Actor"))
                {
                    Actor actor = m_EditorScene.CreateActor("New Actor");
                    m_SelectedActor = actor;
                }
                ImGui.SameLine();
                ImGui.Button("Delete All Actors");

                ImGui.Separator();

                foreach (int actor in m_ActiveScene.EnumarateActors())
                {
                    Actor actorObj = new Actor(actor, m_ActiveScene);

                    ref NameComponent name = ref actorObj.GetComponent<NameComponent>();

                    bool isActorRemoved = false;

                    if (ImGui.TreeNodeEx(new IntPtr(actor),  (m_SelectedActor == actorObj ? ImGuiTreeNodeFlags.Selected : 0), name.Name))
                    {
                        if (ImGui.IsItemClicked())
                        {
                            m_SelectedActor = actorObj;
                        }

                        if (ImGui.BeginPopupContextWindow("ActorSettings"))
                        {
                            if (ImGui.MenuItem("Delete Actor"))
                            {
                                isActorRemoved = true;
                            }

                            ImGui.EndPopup();
                        }

                        ImGui.TreePop();
                    }

                    if (isActorRemoved)
                    {
                        m_ActiveScene.RemoveActor(actorObj);
                    }
                }
            }


            using (new ScopedMenu("Inspector"))
            {
                ref NameComponent actorName = ref m_SelectedActor.GetComponent<NameComponent>();
                IntPtr namePtr = Marshal.StringToHGlobalAnsi(actorName.Name);
                ImGui.InputText("Actor Name", namePtr, 256);
                actorName.Name = Marshal.PtrToStringAnsi(namePtr, 256);
                Marshal.FreeHGlobal(namePtr);

                if (ImGui.Button("Add Component"))
                {
                    ImGui.OpenPopup("AddComponent");
                }

                if (ImGui.BeginPopup("AddComponent"))
                {
                    RenderAddComponent<MeshComponent>("Mesh Component");
                    RenderAddComponent<CameraComponent>("Camera Component");
                    RenderAddComponent<PointLightComponent>("Point Light Component");
                    ImGui.EndPopup();
                }

                RenderComponent("Transform Component", (ref TransformComponent transform) =>
                {
                    Vector3 convertedPosVec = new Vector3(transform.Position.X, transform.Position.Y, transform.Position.Z);
                    ImGui.DragFloat3("Position", ref convertedPosVec, 0.05f);
                    transform.Position = new OpenTK.Mathematics.Vector3(convertedPosVec.X, convertedPosVec.Y, convertedPosVec.Z);

                    Vector3 convertedRotVec = new Vector3(transform.Rotation.X, transform.Rotation.Y, transform.Rotation.Z);
                    ImGui.DragFloat3("Rotation", ref convertedRotVec, 0.05f);
                    transform.Rotation = new OpenTK.Mathematics.Vector3(convertedRotVec.X, convertedRotVec.Y, convertedRotVec.Z);

                    Vector3 convertedScaleVec = new Vector3(transform.Scale.X, transform.Scale.Y, transform.Scale.Z);
                    ImGui.DragFloat3("Scale", ref convertedScaleVec, 0.05f);
                    transform.Scale = new OpenTK.Mathematics.Vector3(convertedScaleVec.X, convertedScaleVec.Y, convertedScaleVec.Z);
                });

                RenderComponent("Mesh Component", (ref MeshComponent mesh) =>
                {
                });

                RenderComponent("Camera Component", (ref CameraComponent camera) =>
                {
                    float fov = camera.FieldOfView;
                    ImGui.DragFloat("FOV", ref fov);
                    camera.FieldOfView = fov;
                });

                RenderComponent("Point Light Component", (ref PointLightComponent pointLight) =>
                {
                    Vector3 convertedColorVec = new Vector3(pointLight.Color.X, pointLight.Color.Y, pointLight.Color.Z);
                    ImGui.ColorEdit3("Light Color", ref convertedColorVec);
                    pointLight.Color = new OpenTK.Mathematics.Vector3(convertedColorVec.X, convertedColorVec.Y, convertedColorVec.Z);

                    float maxDistance = pointLight.MaxDistance;
                    ImGui.DragFloat("Distance", ref maxDistance);
                    pointLight.MaxDistance = maxDistance;
                });
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

                ImGui.Text($"Selected Actor: {m_SelectedActor.GetComponent<NameComponent>().Name}");
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

        private void RenderAddComponent<T>(string name) where T : struct
        {
            if (!m_SelectedActor.HasComponent<T>())
            {
                if (ImGui.Button(name))
                {
                    m_SelectedActor.AddComponent(new T());
                }
            }
        }

        private void RenderComponent<T>(string name, RenderComponentAction<T> renderAction) where T : struct
        {
            if (!m_SelectedActor.HasComponent<T>()) return;

            bool componentTreeOpen = ImGui.TreeNodeEx(name);
            ImGui.SameLine();
            if (ImGui.Button("Remove"))
            {
                m_SelectedActor.RemoveComponent<T>();
                return;
            }

            if (componentTreeOpen)
            {
                ref T component = ref m_SelectedActor.GetComponent<T>();
                renderAction.Invoke(ref component);

                ImGui.TreePop();
            }
        }

        delegate void RenderComponentAction<T>(ref T component);
    }
}
