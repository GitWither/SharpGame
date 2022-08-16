using OpenTK.Graphics.OpenGL4;

using SharpGame.Util;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SharpGame.Graphics
{
    public class Texture
    {
        public int Id { get; }

        private readonly int height;
        private readonly int width;

        public Texture(string file) : this(
            (Bitmap)Image.FromFile(SharedConstants.TextureFolder + file + SharedConstants.TextureExtension))
        {
            Bitmap bitmap;
            try
            {
                bitmap = (Bitmap)Image.FromFile(SharedConstants.TextureFolder + file +
                                                SharedConstants.TextureExtension);
            }
            catch (IOException e)
            {
                Logger.Exception(e);
                return;
            }

            Logger.Info("Successfully loaded texture " + file);
        }

        public Texture(Bitmap bitmap)
        {
            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            Id = GL.GenTexture();
            height = bitmapData.Height;
            width = bitmapData.Width;

            Bind(TextureUnit.Texture0);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, width, height, 0,
                OpenTK.Graphics.OpenGL4.PixelFormat.Bgra, PixelType.UnsignedByte, bitmapData.Scan0);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (float)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (float)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                (float)TextureMinFilter.NearestMipmapLinear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
                (float)TextureMagFilter.Nearest);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            Unbind();


            bitmap.UnlockBits(bitmapData);
            bitmap.Dispose();
        }

        public void Bind(TextureUnit unit)
        {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, Id);
        }

        public void Unbind()
        {
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        ~Texture()
        {
            //GL.DeleteTexture(Id);
        }
    }
}
