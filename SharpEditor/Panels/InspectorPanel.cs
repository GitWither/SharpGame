using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using ImGuiNET;
using SharpEditor.Util;
using SharpGame.Objects;
using SharpGame.Objects.Components;

namespace SharpEditor.Panels
{
    public class InspectorPanel : IPanel
    {
        private readonly ActorsPanel m_Actors;

        public InspectorPanel(ActorsPanel actors)
        {
            this.m_Actors = actors;
        }

        public void OnImGuiRender()
        {
            using (new ScopedMenu("Inspector"))
            {
                ref NameComponent actorName = ref m_Actors.SelectedActor.GetComponent<NameComponent>();
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
        }

        private void RenderAddComponent<T>(string name) where T : struct
        {
            if (m_Actors.SelectedActor.HasComponent<T>()) return;

            if (ImGui.Button(name))
            {
                m_Actors.SelectedActor.AddComponent(new T());
            }
        }

        private void RenderComponent<T>(string name, RenderComponentAction<T> renderAction) where T : struct
        {
            if (!m_Actors.SelectedActor.HasComponent<T>()) return;

            bool componentTreeOpen = ImGui.TreeNodeEx(name);
            ImGui.SameLine();
            if (ImGui.Button("Remove"))
            {
                m_Actors.SelectedActor.RemoveComponent<T>();
                return;
            }

            if (!componentTreeOpen) return;


            ref T component = ref m_Actors.SelectedActor.GetComponent<T>();
            renderAction.Invoke(ref component);

            ImGui.TreePop();
        }

        delegate void RenderComponentAction<T>(ref T component);
    }
}
