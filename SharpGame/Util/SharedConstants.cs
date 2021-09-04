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
        //=============== MISC NUMERIC CONSTS ===============
        public const int MaxActors = 5000;

        public const uint MaxComponents = 128;
        public const uint MaxMeshes = 120;
        public const int MaxLights = 4;

        public const float VectorMargin = 0.01f;

        //=============== ASSET EXTENSIONS ===============
        public const string FragmentShaderExtension = ".frag";
        public const string VertexShaderExtension = ".vert";
        public const string TextureExtension = ".png";
        public const string MeshExtension = ".obj";

        //=============== OBJ TOKEN NAMES ===============
        public const string CommentMeshToken = "#";
        public const string VertexMeshToken = "v";
        public const string TextureMeshToken = "vt";
        public const string NormalMeshToken = "vn";
        public const string FaceMeshToken = "f";

        //=============== LOGGER LEVELS ===============
        public const string LoggerLevelInfo = "INFO";
        public const string LoggerLevelDebug = "DEBUG";
        public const string LoggerLevelError = "ERROR";
        public const string LoggerLevelException = "EXCEPTION";
        public const string LoggerLevelWarn = "WARN";

        //=============== THREAD NAMES ===============
        public const string RenderThreadName = "RENDER";
        public const string LogicThreadName = "LOGIC";

        //=============== SHADER UNIFORM CONSTANTS ===============
        public const string UniformModelViewProjection = "u_ModelViewProjection";
        public const string UniformSpecularity = "u_Specularity";
        public const string UniformModel = "u_Translation";
        public const string UniformHasNormalMap = "u_HasNormalMap";
        public const string UniformHasEmmissionMap = "u_HasEmmissionMap";
        public const string UniformView = "u_View";
        public const string UniformProjection = "u_Projection";

        //=============== PATH CONSTANTS ===============
        public const string ShaderPath = "Shaders/";
        public const string TextureFolder = "Textures/";
        public const string MeshFolder = "Meshes/";
    }
}
