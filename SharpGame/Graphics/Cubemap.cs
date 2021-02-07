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
    public class Cubemap : IDisposable
    {
        private readonly int id;

        public Cubemap(string file)
        {
            id = GL.GenTexture();

            Bind(TextureUnit.Texture0);

            for (int i = 0; i < 6; i++)
            {
                Bitmap bitmap;

                try
                {
                    bitmap = (Bitmap)Image.FromFile(SharedConstants.TextureFolder + file + "_" + i + SharedConstants.TextureExtension);
                }
                catch (IOException e)
                {
                    Logger.Exception(e);
                    return;
                }
                BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX + i, 0, PixelInternalFormat.Rgba, bitmapData.Width, bitmapData.Height, 0, OpenTK.Graphics.OpenGL4.PixelFormat.Bgra, PixelType.UnsignedByte, bitmapData.Scan0);

                GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (float)TextureMagFilter.Linear);
                GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (float)TextureMagFilter.Linear);

                //Fix for rough edges
                GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (float)TextureWrapMode.ClampToEdge);
                GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (float)TextureWrapMode.ClampToEdge);
                GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (float)TextureWrapMode.ClampToEdge);

                bitmap.UnlockBits(bitmapData);
                bitmap.Dispose();
            }

            Unbind();
        }

        public void Bind(TextureUnit unit)
        {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.TextureCubeMap, id);
        }

        public void Dispose()
        {
            GL.DeleteTexture(id);
        }

        public void Unbind()
        {
            GL.BindTexture(TextureTarget.TextureCubeMap, 0);
        }
    }
}
