using OpenTK.Graphics.OpenGL4;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpGame.Graphics
{
    internal class FrameBuffer : IDisposable
    {
        private readonly int id;
        public FrameBuffer(int width, int height)
        {
            id = GL.GenFramebuffer();

            //GL.BindFramebuffer(FramebufferTarget.)
        }

        public void Dispose()
        {
            GL.DeleteFramebuffer(id);
        }
    }
}
