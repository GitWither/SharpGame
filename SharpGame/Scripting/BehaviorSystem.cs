using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpActors;

using SharpGame.Graphics;
using SharpGame.Objects.Components;

namespace SharpGame.Scripting
{
    internal class BehaviorSystem : ActorSystem
    {
        public void OnUpdate()
        {
            foreach (int actor in this.Actors)
            {
                SharpGameWindow.Instance.BehaviorManager.OnUpdate(actor);
            }
        }
    }
}
