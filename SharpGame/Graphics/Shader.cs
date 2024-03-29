﻿using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

using SharpGame.Util;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;


namespace SharpGame.Graphics
{
    public class Shader : IDisposable
    {
        public static readonly Shader Unlit;
        public static readonly Shader Lit;
        public static readonly Shader Gui;
        public static readonly Shader Text;
        public static readonly Shader Particle;
        public static readonly Shader Skybox;
        public static readonly Shader ImGui;

        private readonly int programId;

        public string Name { get; }

        static Shader()
        {
            Unlit = new Shader("unlit");
            Lit = new Shader("lit");
            Gui = new Shader("gui");
            Text = new Shader("text");
            Particle = new Shader("particle");
            Skybox = new Shader("skybox");
            ImGui = new Shader("imgui");
        }

        public Shader(string path) 
            : this(path, File.ReadAllText(SharedConstants.ShaderPath + path + SharedConstants.FragmentShaderExtension), File.ReadAllText(SharedConstants.ShaderPath + path + SharedConstants.VertexShaderExtension))
        {
            Logger.Info("Successfully loaded shader " + path);
        }
        public Shader(string name, string fragmentSource, string vertexSource)
        {
            programId = GL.CreateProgram();
            Name = name;

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

        public void UploadBool(string name, bool value)
        {
            if (value)
            {
                GL.Uniform1(GetUniformLocation(name), 0.5f);
            }
            else 
            {
                GL.Uniform1(GetUniformLocation(name), 0f);
            }
        }

        public void UploadFloat(string name, float value)
        {
            GL.Uniform1(GetUniformLocation(name), value);
        }

        public void UploadFloatArray(string name, Span<float> value)
        {
            GL.Uniform1(GetUniformLocation(name), value.Length, value.ToArray());
        }

        public void UploadInt(string name, int value)
        {
            GL.Uniform1(GetUniformLocation(name), value);
        }

        public void UploadIntArray(string name, int[] value)
        {
            GL.Uniform1(GetUniformLocation(name), value.Length, value);
        }

        public void UploadVector3Array(string name, Span<Vector3> value)
        {
            float[] values = new float[value.Length * 3];
            for (int i = 0; i < values.Length; i += 3)
            {
                int vectorIndex = i / 3;

                values[i] = value[vectorIndex].X;
                values[i + 1] = value[vectorIndex].Y;
                values[i + 2] = value[vectorIndex].Z;
            }
            GL.Uniform3(GetUniformLocation(name), value.Length, values);
        }

        private int GetUniformLocation(string name)
        {
            return GL.GetUniformLocation(programId, name);
        }

        public void Dispose()
        {
            GL.DeleteProgram(programId);
        }

        public void Unbind()
        {
            GL.UseProgram(0);
        }
    }
}
