using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImGuiNET;

namespace SharpEditor.Util
{
    public struct ScopedMenu : IDisposable
    {

        public ScopedMenu(string name)
        {
            ImGui.Begin(name);
        }

        public ScopedMenu(string name, ImGuiWindowFlags windowFlags)
        {
            ImGui.Begin(name, windowFlags);
        }

        public void Dispose()
        {
            ImGui.End();
        }
    }
}
