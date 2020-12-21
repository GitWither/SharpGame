using OpenTK;
using OpenTK.Graphics.OpenGL4;

using SharpGame.Util;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpGame.Graphics
{
    public class Shader : IDisposable
    {
        public static Shader Unlit = new Shader("shader");

        private readonly int programId;

        public Shader(string path) 
            : this(path, File.ReadAllText(path + SharedConstants.FragmentShaderExtension), File.ReadAllText(path + SharedConstants.VertexShaderExtension))
        {
            Logger.Info("Successfully loaded shader " + path);
        }
        public Shader(string name, string fragmentSource, string vertexSource)
        {
            programId = GL.CreateProgram();

            AttachShader(ShaderType.VertexShader, vertexSource);
            AttachShader(ShaderType.FragmentShader, fragmentSource);

            GL.LinkProgram(programId);
            string log = GL.GetProgramInfoLog(programId);
            if (!string.IsNullOrEmpty(log))
            {
                Logger.Error($"There was an error during the link of a shader \"{name}\": {log}");
            }
        }

        public void Bind() => GL.UseProgram(programId);

        private void AttachShader(ShaderType type, string source)
        {
            int id = GL.CreateShader(type);
            GL.ShaderSource(id, source);
            GL.CompileShader(id);
            GL.AttachShader(programId, id);
        }

        public void UploadMatrix4(string name, ref Matrix4 value)
        {
            GL.UniformMatrix4(GetUniformLocation(name), false, ref value);
        }

        public void UploadMatrix3(string name, ref Matrix3 value)
        {
            GL.UniformMatrix3(GetUniformLocation(name), false, ref value);
        }

        public void UploadVector2(string name, ref Vector2 value)
        {
            GL.Uniform2(GetUniformLocation(name), value);
        }

        public void UploadVector3(string name, ref Vector3 value)
        {
            GL.Uniform3(GetUniformLocation(name), value);
        }

        public void UploadVector4(string name, ref Vector4 value)
        {
            GL.Uniform4(GetUniformLocation(name), value);
        }

        public void UploadFloat(string name, float value)
        {
            GL.Uniform1(GetUniformLocation(name), value);
        }

        public void UploadInt(string name, int value)
        {
            GL.Uniform1(GetUniformLocation(name), value);
        }

        private int GetUniformLocation(string name)
        {
            return GL.GetUniformLocation(programId, name);
        }

        public void Dispose()
        {
            GL.DeleteProgram(programId);
        }
    }
}
