using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using SharpGame.Graphics.Vaos;
using SharpGame.Util;

namespace SharpGame.Graphics
{
    internal static class RenderCommand
    {
        public static void DrawIndexed(VertexArrayObject vao)
        {
            vao.Bind();
            GL.DrawElements(PrimitiveType.Triangles, vao.IndexCount, DrawElementsType.UnsignedInt, 0);
        }
    }
}
