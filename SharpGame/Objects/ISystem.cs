using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpGame.Objects
{
    public interface ISystem
    {
        void OnAwake();
        void OnUpdate(List<Actor> actors, float deltaTime);
        void OnRender(List<Actor> actors);
        void OnShutdown();
    }
}
