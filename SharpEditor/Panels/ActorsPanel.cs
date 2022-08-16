using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImGuiNET;
using SharpEditor.Util;
using SharpGame.Objects;
using SharpGame.Objects.Components;

namespace SharpEditor.Panels
{
    public class ActorsPanel : IPanel
    {
        public Scene Scene { get; set; }
        public Actor SelectedActor { get; set; }
        public void OnImGuiRender()
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

                foreach (int actor in Scene.EnumarateActors())
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
