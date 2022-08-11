using OpenTK.Graphics.OpenGL4;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpGame.Util;
using GL = OpenTK.Graphics.OpenGL4.GL;

namespace SharpGame.Graphics
{
    public class Framebuffer : IDisposable
    {
        private int m_Id;

        private int m_Width;
        private int m_Height;
        private int m_Samples;
        private bool m_SwapChain = false;
        private int m_ColorAttachment, m_DepthAttachment;

        public int ColorAttachment => m_ColorAttachment;

        public Framebuffer(int width, int height, int samples)
        {
            this.m_Width = width;
            this.m_Height = height;
            this.m_Samples = samples;

            Reset();
        }

        private void Reset()
        {
            m_Id = GL.GenFramebuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, m_Id);

            GL.CreateTextures(TextureTarget.Texture2D, 1, out m_ColorAttachment);
            GL.BindTexture(TextureTarget.Texture2D, m_ColorAttachment);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba8, m_Width, m_Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (float)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (float)TextureMagFilter.Linear);

            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, m_ColorAttachment, 0);

            GL.CreateTextures(TextureTarget.Texture2D, 1, out m_DepthAttachment);
            GL.BindTexture(TextureTarget.Texture2D, m_DepthAttachment);
            //GL.TexStorage2D(TextureTarget2d.Texture2D, 1, SizedInternalFormat.R32i, m_Width, m_Height);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Depth24Stencil8, m_Width, m_Height, 0, PixelFormat.DepthStencil, PixelType.UnsignedInt248, IntPtr.Zero);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthStencilAttachment, TextureTarget.Texture2D, m_DepthAttachment, 0);

            if (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != FramebufferErrorCode.FramebufferComplete)
            {
                Logger.Error($"Framebuffer with ID {m_Id} is incomplete");
            }

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        public void Resize(int width, int height)
        {
            this.m_Width = width;
            this.m_Height = height;

            Reset();
        }

        public int Read(int attachment, int x, int y)
        {
            GL.ReadBuffer(ReadBufferMode.ColorAttachment0 + attachment);
            int pixelData = 0;
            GL.ReadPixels(x, y, 1, 1, PixelFormat.RedInteger, PixelType.Int, ref pixelData);
            return pixelData;
        }

        public void Bind()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, m_Id);
            GL.Viewport(0, 0, m_Width, m_Height);
        }

        public void Unbind()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        public void Dispose()
        {
            GL.DeleteFramebuffer(m_Id);
        }
    }
}
