using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ImGuiNET;
using SharpEditor.Util;

using SharpGame.Graphics;

namespace SharpEditor.Panels
{
    internal class MaterialPanel : IPanel
    {
        private static readonly Dictionary<string, Shader> NameToShader = new()
        {
            {"pbr", Shader.Pbr},
            {"unlit", Shader.Unlit},
            {"lit", Shader.Lit}
        };

        public Material? ContextMaterial { get; set; }
        public void OnImGuiRender(ref bool open)
        {
            using (new ScopedMenu("Material Editor", ref open))
            {
                if (ContextMaterial == null)
                {
                    ImGui.Text("No material selected!");
                    return;
                }

                if (ImGui.BeginCombo("Shader", ContextMaterial.Shader.Name))
                {
                    foreach (KeyValuePair<string, Shader> nameShaderPair in NameToShader)
                    {
                        if (ImGui.Selectable(nameShaderPair.Key))
                        {
                            ContextMaterial.Shader = NameToShader[nameShaderPair.Key];
                        }
                    }

                    ImGui.EndCombo();
                }

                ImGui.Text("Albedo");
                ImGui.SameLine();
                ImGui.ImageButton((IntPtr)ContextMaterial.BaseMap.Id, new Vector2(50));
            }
        }
    }
}
