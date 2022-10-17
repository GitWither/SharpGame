using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImGuiNET;
using SharpEditor.Util;
using SharpGame.Objects;
using SharpGame.Objects.Components;
using SharpGame.Util;

namespace SharpEditor.Panels
{
    public class ActorsPanel : IPanel
    {
        private Scene m_ContextScene = null;

        public Scene Scene
        {
            get => m_ContextScene;
            set
            {
                m_ContextScene = value;
                this.SelectedActor = Actor.Null;
            }
        }
        public Actor SelectedActor { get; set; }
        public void OnImGuiRender(ref bool open)
        {
            using (new ScopedMenu("Actors"))
            {
                if (ImGui.Button("Create Actor"))
                {
                    Actor actor = Scene.CreateActor("New Actor");
                    SelectedActor = actor;
                }
                ImGui.SameLine();
                ImGui.Button("Delete All Actors");

                ImGui.Separator();

                foreach (int actor in Scene.EnumerateActors())
                {
                    Actor actorObj = new Actor(actor, Scene);

                    ref NameComponent name = ref actorObj.GetComponent<NameComponent>();

                    bool isActorRemoved = false;

                    if (ImGui.TreeNodeEx(new IntPtr(actor), (SelectedActor == actorObj ? ImGuiTreeNodeFlags.Selected : 0), name.Name))
                    {
                        if (ImGui.IsItemClicked())
                        {
                            SelectedActor = actorObj;
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
                        Scene.RemoveActor(actorObj);
                        SelectedActor = Actor.Null;
                    }
                }
            }
        }
    }
}
