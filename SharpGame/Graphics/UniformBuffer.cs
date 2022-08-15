using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using SharpGame.Util;

namespace SharpGame.Graphics
{
    public class UniformBuffer
    {
        private readonly int m_Id;

        public UniformBuffer(int size, int binding)
        {
            GL.CreateBuffers(1, out m_Id);
            GL.NamedBufferData(m_Id, size, IntPtr.Zero, BufferUsageHint.DynamicDraw);
            GL.BindBufferBase(BufferRangeTarget.UniformBuffer, binding, m_Id);
        }

        ~UniformBuffer()
        {
            GL.DeleteBuffer(m_Id);
        }

        public void UploadData<T>(ref T data, int size, int offset) where T : struct
        {
            GL.NamedBufferSubData(m_Id, new IntPtr(offset), size, ref data);
        }
    }
}
