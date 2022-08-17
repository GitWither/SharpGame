using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

using SharpGame.Graphics.Meshes;
using SharpGame.Graphics.Vaos;
using SharpGame.Objects.Components;
using SharpGame.Util;

namespace SharpGame.Graphics
{
    public class Renderer
    {
        public Matrix4 m_ViewProjection;

        private UniformBuffer m_CameraUniformBuffer;


        private Dictionary<Material, List<RawMesh>> m_Meshes;

        public int DrawCalls { get; private set; }
        public int Vertices { get; private set; }
        public int Indices { get; private set; }

        public void Initialize()
        { ;
            m_CameraUniformBuffer = new UniformBuffer(Unsafe.SizeOf<Matrix4x4>(), 0);
            m_Meshes = new Dictionary<Material, List<RawMesh>>();

            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.DepthTest);
            GL.Disable(EnableCap.CullFace);

            GL.Enable(EnableCap.DebugOutput);
            GL.Enable(EnableCap.DebugOutputSynchronous);
            GL.DebugMessageCallback((source, type, id, severity, length, message, userParam) =>
            {
                string msg = System.Runtime.InteropServices.Marshal.PtrToStringAnsi(message, length);
                    switch (severity)
                    {
                        case DebugSeverity.DebugSeverityHigh:
                            Logger.Exception(new Exception(msg));
                            break;
                        case DebugSeverity.DebugSeverityLow:
                            Logger.Warn(msg);
                            break;
                        case DebugSeverity.DebugSeverityMedium:
                            Logger.Error(msg);
                            break;
                        case DebugSeverity.DebugSeverityNotification:
                            Logger.Info(msg);
                            break;
                        case DebugSeverity.DontCare:
                            Logger.Debug(msg);
                            break;
                    }
                }, IntPtr.Zero);

            //GL.DebugMessageControl(DebugSourceControl.DontCare, DebugTypeControl.DontCare, DebugSeverityControl.DebugSeverityNotification, 0, (int*)IntPtr.Zero, false);
        }

        public void Begin(Camera camera)
        {
            Vertices = 0;
            DrawCalls = 0;
            Indices = 0;
            m_ViewProjection = camera.View * camera.Projection;
            m_CameraUniformBuffer.UploadData(ref m_ViewProjection, Unsafe.SizeOf<Matrix4x4>(), 0);

            RenderCommand.Clear();
        }

        public void DrawMesh(Material material, Mesh mesh, TransformComponent transform)
        {
            if (mesh == null || material == null) return;
            if (m_Meshes.TryGetValue(material, out List<RawMesh> list))
            {
                list.Add(new RawMesh(mesh.VertexArray, MathUtil.CreateTransformationMatrix(transform)));
            }
            else
            {
                m_Meshes.Add(material, new List<RawMesh> { new(mesh.VertexArray, MathUtil.CreateTransformationMatrix(transform)) });
            }

            Vertices += mesh.VertexArray.VertexCount;
            Indices += mesh.VertexArray.IndexCount;
        }

        public void End()
        {
            Flush();
        }

        private void Flush()
        {
            foreach (KeyValuePair<Material, List<RawMesh>> material in m_Meshes)
            {
                Shader shader = material.Key.Shader;
                shader.Bind();
                material.Key.BaseMap.Bind(TextureUnit.Texture0);

                foreach (RawMesh mesh in material.Value)
                {
                    Matrix4 transform = mesh.Transform;
                    shader.UploadMatrix4(SharedConstants.UniformModel, ref transform);

                    RenderCommand.DrawIndexed(mesh.VertexArray);
                    DrawCalls++;
                }
            }
            m_Meshes.Clear();
        }
    }

    internal struct RawMesh
    {
        public VertexArrayObject VertexArray { get; }
        public Matrix4 Transform { get; }

        public RawMesh(VertexArrayObject vertexArray, Matrix4 transform)
        {
            this.VertexArray = vertexArray;
            this.Transform = transform;
        }
    }
}
