using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImGuiNET;

namespace SharpEditor.Util
{
    public struct ScopedId : IDisposable
    {
        public ScopedId(string id)
        {
            ImGui.PushID(id);
        }

        public ScopedId(int id)
        {
            ImGui.PushID(id);
        }

        public ScopedId(IntPtr id)
        {
            ImGui.PushID(id);
        }

        public void Dispose()
        {
            ImGui.PopID();
        }
    }
}
