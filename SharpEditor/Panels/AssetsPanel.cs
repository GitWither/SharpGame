using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImGuiNET;
using SharpEditor.Util;

namespace SharpEditor.Panels
{
    public class AssetsPanel : IPanel
    {
        private string m_CurrentDirectory = ".";
        public void OnImGuiRender()
        {
            using (new ScopedMenu("Assets"))
            {
                if (ImGui.Button("<"))
                {
                    m_CurrentDirectory = Directory.GetParent(m_CurrentDirectory).FullName;
                }
                ImGui.Columns(5, "cool");
                foreach (string path in Directory.EnumerateFileSystemEntries(m_CurrentDirectory))
                {
                    if (Directory.Exists(path))
                    {
                        if (ImGui.Button(Path.GetFileNameWithoutExtension(path)))
                        {
                            m_CurrentDirectory = Path.Combine(m_CurrentDirectory, path);
                        }
                    }
                    else
                    {
                        ImGui.Text(Path.GetFileName(path));
                    }

                    ImGui.NextColumn();
                }
            }
        }
    }
}
