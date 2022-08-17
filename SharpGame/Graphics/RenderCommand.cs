using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using SharpGame.Graphics.Vaos;
using SharpGame.Util;

namespace SharpGame.Graphics
{
    public static class RenderCommand
    {
        public static void SetColor(Vector3 color)
        {
            GL.ClearColor(color.X, color.Y, color.Z, 255f);
        }
        public static void Clear()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);
        }

        public static void DrawIndexed(VertexArrayObject vao)
        {
            vao.Bind();
            GL.DrawElements(PrimitiveType.Triangles, vao.IndexCount, DrawElementsType.UnsignedInt, 0);
        }
    }
}
