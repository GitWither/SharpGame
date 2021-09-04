using OpenTK.Mathematics;

using System;
using System.Collections.Generic;
using System.Text;

namespace SharpGame.Events
{
    public class ScreenEventArgs : EventArgs
    {
        public Vector2i Size { get; }
        public ScreenEventArgs(Vector2i size)
        {
            this.Size = size;
        }
    }
}
