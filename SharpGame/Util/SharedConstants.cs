using SharpGame.Objects;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpGame.Util
{
    internal static class SharedConstants
    {
        public const uint MaxComponents = 128;
        public const int MaxActors = 5000;
        public const uint MaxMeshes = 120;
        public const uint MaxLights = 4;

        public const float VectorMargin = 0.01f;

        public const string FragmentShaderExtension = ".frag";
        public const string VertexShaderExtension = ".vert";
        public const string TextureExtension = ".png";
        public const string MeshExtension = ".obj";

        public const string CommentMeshToken = "#";
        public const string VertexMeshToken = "v";
        public const string TextureMeshToken = "vt";
        public const string NormalMeshToken = "vn";
        public const string FaceMeshToken = "f";

        public const string LoggerLevelInfo = "INFO";
        public const string LoggerLevelDebug = "DEBUG";
        public const string LoggerLevelError = "ERROR";
        public const string LoggerLevelException = "EXCEPTION";
        public const string LoggerLevelWarn = "WARN";

        public const string RenderThreadName = "RENDER";
        public const string MainThreadName = "MAIN";

        public const string UniformModelViewProjection = "u_ModelViewProjection";
        public const string UniformTranslationMatrix = "u_Translation";

        public const string ShaderPath = "Shaders/";
        public const string TextureFolder = "Textures/";
        public const string MeshFolder = "Meshes/";
    }
}
