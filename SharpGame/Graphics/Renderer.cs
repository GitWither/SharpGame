using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using SharpGame.Graphics.Meshes;
using SharpGame.Graphics.Vaos;
using SharpGame.Objects.Components;
using SharpGame.Objects.Components.Transform;
using SharpGame.Util;

namespace SharpGame.Graphics
{
    public class Renderer
    {
        private Matrix4 m_ViewProjection;


        private Dictionary<Material, List<RawMesh>> m_Meshes;

        public void Initialize()
        {
            m_Meshes = new Dictionary<Material, List<RawMesh>>();

            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.DepthTest);
            GL.Disable(EnableCap.CullFace);

            GL.Enable(EnableCap.DebugOutput);
            GL.Enable(EnableCap.DebugOutputSynchronous);
            GL.DebugMessageCallback((DebugSource source,
                    DebugType type,
                    int id,
                    DebugSeverity severity,
                    int length,
                    IntPtr message,
                    IntPtr userParam) =>
            {
                string msg = System.Runtime.InteropServices.Marshal.PtrToStringAnsi(message);
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
            m_ViewProjection = camera.View * camera.Projection;
        }

        public void DrawMesh(Material material, Mesh mesh, TransformComponent transform)
        {
            if (m_Meshes.TryGetValue(material, out List<RawMesh> list))
            {
                list.Add(new RawMesh(mesh.VertexArray, MathUtil.CreateTransformationMatrix(transform)));
            }
            else
            {
                m_Meshes.Add(material, new List<RawMesh> { new(mesh.VertexArray, MathUtil.CreateTransformationMatrix(transform)) });
            }
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
                shader.UploadMatrix4(SharedConstants.UniformViewProjection, ref m_ViewProjection);

                foreach (RawMesh mesh in material.Value)
                {
                    Matrix4 transform = mesh.Transform;
                    shader.UploadMatrix4(SharedConstants.UniformModel, ref transform);

                    RenderCommand.DrawIndexed(mesh.VertexArray);
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
