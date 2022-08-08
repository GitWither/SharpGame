using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpGame.Core
{
    public interface ILayer
    {
        void OnDetach();
        void OnAttach();
        void OnUpdate(float deltaTime);
        void OnRender();
    }
}
