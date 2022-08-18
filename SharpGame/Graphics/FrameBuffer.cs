using OpenTK.Graphics.OpenGL4;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.CompilerServices;
using OpenTK.Mathematics;
using SharpGame.Util;

namespace SharpGame.Graphics
{
    public class Framebuffer : IDisposable
    {
        private int m_Id;

        private FramebufferInfo m_Info;

        private int[] m_ColorAttachments; 
        private int m_DepthAttachment;

        public int Width => m_Info.Width;
        public int Height => m_Info.Height;


        public Framebuffer(FramebufferInfo info)
        {
            this.m_Info = info;

            this.m_ColorAttachments = new int[m_Info.ColorAttachments.Length];

            Reset();
        }

        public int GetColorAttachment(int index = 0)
        {
            return m_ColorAttachments[index];
        }

        private void Reset()
        {
            if (m_Id != 0)
            {
                this.Dispose();
            }

            GL.CreateFramebuffers(1, out m_Id);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, m_Id);


            bool multisample = m_Info.Samples > 1;

            if (m_Info.ColorAttachments.Length > 0)
            {
                FramebufferUtils.CreateTextures(multisample, m_ColorAttachments, m_ColorAttachments.Length);

                for (int i = 0; i < m_ColorAttachments.Length; i++)
                {
                    FramebufferUtils.BindTexture(multisample, m_ColorAttachments[i]);
                    switch (m_Info.ColorAttachments[i].Format)
                    {
                        case FramebufferTextureFormat.RedInteger:
                            FramebufferUtils.AttachColorTexture(m_ColorAttachments[i], m_Info.Samples, InternalFormat.R32i, PixelFormat.RedInteger, m_Info.Width, m_Info.Height, i);
                            break;
                        case FramebufferTextureFormat.Rgba8:
                            FramebufferUtils.AttachColorTexture(m_ColorAttachments[i], m_Info.Samples, InternalFormat.Rgba8, PixelFormat.Rgba, m_Info.Width, m_Info.Height, i);
                            break;
                        case FramebufferTextureFormat.Rgba16f:
                            FramebufferUtils.AttachColorTexture(m_ColorAttachments[i], m_Info.Samples, InternalFormat.Rgba16f, PixelFormat.Rgba, m_Info.Width, m_Info.Height, i);
                            break;
                    }
                }
            }

            if (m_Info.DepthAttachment.Format != FramebufferTextureFormat.None)
            {
                FramebufferUtils.CreateTextures(multisample, out m_DepthAttachment, 1);
                FramebufferUtils.BindTexture(multisample, m_DepthAttachment);

                switch (m_Info.DepthAttachment.Format)
                {
                    case FramebufferTextureFormat.Depth24Stencil8:
                        FramebufferUtils.AttachDepthTexture(m_DepthAttachment, m_Info.Samples, PixelInternalFormat.Depth24Stencil8, FramebufferAttachment.DepthStencilAttachment, m_Info.Width, m_Info.Height);
                        break;
                }
            }

            if (m_ColorAttachments.Length > 1)
            {
                DrawBuffersEnum[] buffers = {
                    DrawBuffersEnum.ColorAttachment0, DrawBuffersEnum.ColorAttachment1,
                    DrawBuffersEnum.ColorAttachment2, DrawBuffersEnum.ColorAttachment3
                };
                GL.DrawBuffers(m_ColorAttachments.Length, buffers);
            }
            else if (m_ColorAttachments.Length == 0)
            {
                GL.DrawBuffer(DrawBufferMode.None);
            }

            if (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != FramebufferErrorCode.FramebufferComplete)
            {
                Logger.Error($"Framebuffer with ID {m_Id} is incomplete");
            }

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        public void Resize(int width, int height)
        {
            this.m_Info.Width = width;
            this.m_Info.Height = height;

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
            GL.Viewport(0, 0, m_Info.Width, m_Info.Height);
        }

        public void Unbind()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        public void Dispose()
        {
            GL.DeleteFramebuffer(m_Id);
            GL.DeleteTextures(m_ColorAttachments.Length, m_ColorAttachments);
            GL.DeleteTextures(1, ref m_DepthAttachment);
        }
    }

    public enum FramebufferTextureFormat
    {
        None,

        Rgba8,
        Rgba16f,
        RedInteger,

        Depth24Stencil8,
    }

    public readonly struct FramebufferAttachmentInfo
    {
        public FramebufferTextureFormat Format { get; }

        public FramebufferAttachmentInfo(FramebufferTextureFormat format)
        {
            this.Format = format;
        }

        public static implicit operator FramebufferAttachmentInfo(FramebufferTextureFormat format)
        {
            return new FramebufferAttachmentInfo(format);
        }

    }

    public struct FramebufferInfo
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int Samples { get; set; }
        public Vector4 ClearColor { get; set; }
        public FramebufferAttachmentInfo[] ColorAttachments { get; set; }
        public FramebufferAttachmentInfo DepthAttachment { get; set; }
    }

    static class FramebufferUtils
    {
        public static bool IsDepth(this FramebufferTextureFormat format)
        {
            switch (format)
            {
                case FramebufferTextureFormat.Depth24Stencil8: return true;
            }

            return false;
        }

        public static void AttachColorTexture(int id, int samples, InternalFormat format, PixelFormat pixelFormat, int width, int height, int index)
        {
            bool multisampled = samples > 1;
            if (multisampled)
            {
                GL.TexImage2DMultisample(TextureTargetMultisample.Texture2DMultisample, samples, (PixelInternalFormat)format, width, height, false);
            }
            else
            {
                GL.TexImage2D(TextureTarget.Texture2D, 0, (PixelInternalFormat)format, width, height, 0, pixelFormat, PixelType.UnsignedByte, IntPtr.Zero);

                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (float)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (float)TextureMagFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapR, (float)TextureWrapMode.ClampToEdge);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (float)TextureWrapMode.ClampToEdge);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (float)TextureWrapMode.ClampToEdge);
            }

            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0 + index, GetTextureTarget(multisampled), id, 0);
        }

        public static void AttachDepthTexture(int id, int samples, PixelInternalFormat format, FramebufferAttachment attachment, int width, int height)
        {
            bool multisampled = samples > 1;
            if (multisampled)
            {
                GL.TexImage2DMultisample(TextureTargetMultisample.Texture2DMultisample, samples, format, width, height, false);
            }
            else
            {
                GL.TexStorage2D(TextureTarget2d.Texture2D, 1, (SizedInternalFormat)format, width, height);

                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (float)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (float)TextureMagFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapR, (float)TextureWrapMode.ClampToEdge);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (float)TextureWrapMode.ClampToEdge);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (float)TextureWrapMode.ClampToEdge);
            }

            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, attachment, GetTextureTarget(multisampled), id, 0);
        }

        public static void CreateTextures(bool multisampled, int[] ids, int count)
        {
            GL.CreateTextures(GetTextureTarget(multisampled), count, ids);
        }

        public static void CreateTextures(bool multisampled, out int ids, int count)
        {
            GL.CreateTextures(GetTextureTarget(multisampled), count, out ids);
        }

        public static void BindTexture(bool multisampled, int id)
        {
            GL.BindTexture(GetTextureTarget(multisampled), id);
        }

        public static TextureTarget GetTextureTarget(bool multisampled)
        {
            return multisampled
                ? TextureTarget.Texture2DMultisample
                : TextureTarget.Texture2D;
        }
    }
}
