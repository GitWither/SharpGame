using SharpGame.Graphics.Vaos;
using SharpGame.Objects;
using SharpGame.Objects.Components;
using SharpGame.Util;

using System.Collections.Generic;

namespace SharpGame.Graphics
{
    internal class Layer
    {
        private readonly List<Actor> actors;

        public Layer()
        {
            this.actors = new List<Actor>();
        }

        public void Add(Actor actor)
        {
            actors.Add(actor);
        }

        public void Remove(Actor actor)
        {
            actors.Remove(actor);
        }

        public void Render()
        {
            for (int i = 0; i < actors.Count; i++)
            {
                if (actors[i].HasComponent<MeshRendererComponent>())
                {
                    actors[i].GetComponent<MeshRendererComponent>().Render();
                }
            }
        }

        internal void Dispose()
        {
            Logger.Info(actors.Count);
        }
    }
}
