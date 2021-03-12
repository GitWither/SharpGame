using OpenTK.Graphics.OpenGL4;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpGame.Graphics
{
    internal class Framebuffer : IDisposable
    {
        private readonly int id;

        private readonly int width;
        private readonly int height;
        public Framebuffer(int width, int height)
        {
            this.width = width;
            this.height = height;

            id = GL.GenFramebuffer();

            //GL.BindFramebuffer(FramebufferTarget.)

           // if (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) == FramebufferErrorCode.FramebufferComplete)  
        }

        public void Bind()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, id);
        }

        public void Unbind()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        public void Dispose()
        {
            GL.DeleteFramebuffer(id);
        }
    }
}
