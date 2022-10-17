using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpGame.Graphics;
using SharpGame.Graphics.Meshes;

namespace SharpGame.Assets
{
    public abstract class Asset : IDisposable
    {
        public abstract void Dispose();
    }
}
