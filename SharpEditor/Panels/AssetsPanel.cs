using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImGuiNET;
using System.Numerics;
using SharpEditor.Util;
using SharpGame.Graphics;
using SharpGame.Util;

namespace SharpEditor.Panels
{
    public class AssetsPanel : IPanel
    {
        private string m_CurrentDirectory = Directory.GetCurrentDirectory();

        private float thumbnailSize = 80f;
        private float padding = 18f;

        private Texture m_FolderIcon = new Texture("EditorAssets/Folder");
        private Texture m_FileIcon = new Texture("EditorAssets/File");

        private string[] m_ParentPaths = Array.Empty<string>();

        public AssetsPanel()
        {
            this.NavigateToPath(m_CurrentDirectory);
        }

        public void OnImGuiRender()
        {
            ImGui.ShowDemoWindow();
            using (new ScopedMenu("Assets"))
            {
                ImGui.Button("Import File");
                ImGui.SameLine();
                ImGui.Button("Open in File Explorer");

                ImGui.Separator();

                if (ImGui.Button("<"))
                {
                    NavigateToPath(Directory.GetParent(m_CurrentDirectory).FullName);
                }

                ImGui.SameLine();


                using (new ScopedStyle(ImGuiStyleVar.ItemSpacing, Vector2.Zero))
                using (new ScopedStyle(ImGuiStyleVar.FramePadding, new Vector2(0.5f, 2)))
                using (new ScopedColor(ImGuiCol.Button, 0x00000000))
                {
                    foreach (string path in m_ParentPaths)
                    {
                        string pathName = Path.GetFileName(path);
                        if (pathName != null)
                        {
                            if (ImGui.Button(pathName))
                            {
                                NavigateToPath(path);
                            }
                            ImGui.SameLine();
                            ImGui.Text("/");
                            ImGui.SameLine();
                        }
                    }
                }


                ImGui.NewLine();
                //ImGui.Text(m_CurrentDirectory);

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
                        NavigateToPath(path);
                    }

                    ImGui.TextWrapped(Path.GetFileName(path));

                    ImGui.NextColumn();
                }
            }
        }

        private void NavigateToPath(string newPath)
        {
            m_CurrentDirectory = newPath;

            m_ParentPaths = new string[newPath.Split(Path.DirectorySeparatorChar).Length];
            DirectoryInfo info = new DirectoryInfo(newPath);
            for (int i = m_ParentPaths.Length - 1; i > -1; i--)
            {
                m_ParentPaths[i] = info.FullName;
                if (info.Parent != null)
                {
                    info = info.Parent;
                }
            }
        }
    }
}
