using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ImGuiNET;

namespace SharpEditor.Util
{
    public struct ScopedColor : IDisposable
    {
        public ScopedColor(ImGuiCol flags, uint color)
        {
            ImGui.PushStyleColor(flags, color);
        }

        public ScopedColor(ImGuiCol flags, Vector4 color)
        {
            ImGui.PushStyleColor(flags, color);
        }

        public void Dispose()
        {
            ImGui.PopStyleColor();
        }
    }
}
