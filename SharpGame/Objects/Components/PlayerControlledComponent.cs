using OpenTK.Graphics.OpenGL4;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpGame.Input;
using OpenTK;
using SharpGame.Util;
using OpenTK.Mathematics;

namespace SharpGame.Objects.Components
{
    public struct PlayerControlledComponent
    {
        public float Speed { get; set; }

        public PlayerControlledComponent()
        {
            this.Speed = 5;
        }
    }
}
