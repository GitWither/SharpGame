using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImGuiNET;
using System.Numerics;
using SharpEditor.Util;
using SharpGame.Graphics;

namespace SharpEditor.Panels
{
    public class AssetsPanel : IPanel
    {
        private string m_CurrentDirectory = ".";

        private float thumbnailSize = 80f;
        private float padding = 18f;

        private Texture m_FolderIcon = new Texture("EditorAssets/Folder");
        private Texture m_FileIcon = new Texture("EditorAssets/File");

        public void OnImGuiRender()
        {
            using (new ScopedMenu("Assets"))
            {
                if (ImGui.Button("<"))
                {
                    m_CurrentDirectory = Directory.GetParent(m_CurrentDirectory).FullName;
                }

                ImGui.SameLine();

                ImGui.Text(m_CurrentDirectory);

                //ImGui.DragFloat("Thumb Size", ref thumbnailSize);
                //ImGui.DragFloat("Padding", ref padding);
                
                Vector2 maxRegion = ImGui.GetContentRegionAvail();
                int maxColumns = (int)(maxRegion.X / (thumbnailSize + padding));

                ImGui.Columns(maxColumns, "FileColumns", false);
                foreach (string path in Directory.EnumerateFileSystemEntries(m_CurrentDirectory))
                {

                    bool isDirectory = Directory.Exists(path);

                    using (new ScopedColor(ImGuiCol.Button, 0x00000000))
                    {
                        ImGui.ImageButton(new IntPtr(isDirectory ? m_FolderIcon.Id : m_FileIcon.Id),
                            new Vector2(thumbnailSize), new Vector2(0, 0), new Vector2(1, 1));
                    }

                    if (isDirectory && ImGui.IsItemHovered() && ImGui.IsMouseDoubleClicked(ImGuiMouseButton.Left))
                    {
                        m_CurrentDirectory = path;
                    }

                    ImGui.TextWrapped(Path.GetFileName(path));

                    ImGui.NextColumn();
                }
            }
        }
    }
}
