using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImGuiNET;
using OpenTK.Mathematics;
using Vector2 = System.Numerics.Vector2;

namespace SharpEditor.Util
{
    public struct ScopedStyle : IDisposable
    {
        public ScopedStyle(ImGuiStyleVar styleVar, float value)
        {
            ImGui.PushStyleVar(styleVar, value);
        }

        public ScopedStyle(ImGuiStyleVar styleVar, Vector2 value)
        {
            ImGui.PushStyleVar(styleVar, value);
        }

        public void Dispose()
        {
            ImGui.PopStyleVar();
        }
    }
}
