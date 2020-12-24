using OpenTK;
using OpenTK.Graphics.ES11;

using SharpGame.Util;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpGame.Graphics.Meshes
{
    public class Mesh
    {
        public Vector3[] Vertices { get; set; }
        public Vector2[] FaceTexCoords { get; set; }
        public Vector3[] Normals { get; set; }
        public uint[] FaceIndices { get; set; }
        public Mesh(Vector3[] vertices, Vector3[] normals, Vector2[] texCoords, uint[] faceIndices)
        {
            this.Vertices = vertices;
            this.FaceTexCoords = texCoords;
            this.FaceIndices = faceIndices;
            this.Normals = normals;
        }

        //I don't know why, I don't want to know why, I shouldn't have to wonder why, but this piece of code was the most painful thing I ever worked with
        public static Mesh FromOBJ(string file)
        {
            List<Vector3> tempVertices = new List<Vector3>();
            List<Vector2> tempUVs = new List<Vector2>();
            List<Vector3> tempNormals = new List<Vector3>();

            List<uint> vertexIndices = new List<uint>();
            List<uint> uvIndices = new List<uint>();
            List<uint> normalIndices = new List<uint>();

            foreach (string line in File.ReadLines(SharedConstants.MeshFolder + file + SharedConstants.MeshExtension))
            {
                string type = line.Substring(0, line.IndexOf(' '));
                string[] data = line.Substring(line.IndexOf(' ') + 1).Split(' ');
                switch (type)
                {
                    case SharedConstants.VertexMeshToken:
                        if (float.TryParse(data[0], out float vertexX) &&
                            float.TryParse(data[1], out float vertexY) &&
                            float.TryParse(data[2], out float vertexZ))
                        {
                            tempVertices.Add(new Vector3(vertexX, vertexY, vertexZ));
                        }
                        break;
                    case SharedConstants.TextureMeshToken:
                        if (float.TryParse(data[0], out float u) &&
                            float.TryParse(data[1], out float v))
                        {
                            tempUVs.Add(new Vector2(u, -v));
                        }
                        break;
                    case SharedConstants.NormalMeshToken:
                        if (float.TryParse(data[0], out float normalX) &&
                            float.TryParse(data[1], out float normalY) &&
                            float.TryParse(data[2], out float normalZ))
                        {
                            tempNormals.Add(new Vector3(normalX, normalY, normalZ));
                        }
                        break;
                    case SharedConstants.FaceMeshToken:
                        if (data.Length < 3) Logger.Exception(new Exception("Detected face with less than 3 vertices"));

                        for (byte i = 0; i < data.Length; i++)
                        {
                            string[] vertexValues = data[i].Split('/');

                            if (uint.TryParse(vertexValues[0], out uint index) && 
                                uint.TryParse(vertexValues[1], out uint texIndex) &&
                                uint.TryParse(vertexValues[2], out uint normalIndex))
                            {
                                vertexIndices.Add(index);
                                uvIndices.Add(texIndex);
                                normalIndices.Add(normalIndex);
                            }
                        }
                        break;
                    default:
                        Logger.Warn($"Object of type {type} is not yet supported!");
                        break;
                }
            }

            List<Vector3> in_vertices = new List<Vector3>();
            List<Vector2> in_uvs = new List<Vector2>();
            List<Vector3> in_normals = new List<Vector3>();

            for (int i = 0; i < vertexIndices.Count; i++)
            {
                uint vertexIndex = vertexIndices[i];
                uint texIndex = uvIndices[i];
                uint normalIndex = normalIndices[i];

                Vector3 vertex = tempVertices[(int)(vertexIndex - 1)];
                Vector3 normal = tempNormals[(int)(normalIndex - 1)];
                Vector2 texCoord = tempUVs[(int)(texIndex - 1)];
                in_vertices.Add(vertex);
                in_normals.Add(normal);
                in_uvs.Add(texCoord);
            }

            List<Vector3> out_vertices = new List<Vector3>();
            List<Vector2> out_uvs = new List<Vector2>();
            List<Vector3> out_normals = new List<Vector3>();
            List<uint> out_indices = new List<uint>();
            ushort indexX = 0;
            for (uint i = 0; i < in_vertices.Count; i++)
            {
                bool yes = getSimilarVertexindex(in_vertices[(int)i], in_normals[(int)i], in_uvs[(int)i], ref out_vertices, ref out_normals, ref out_uvs, ref indexX);

                
                if (yes)
                {
                    out_indices.Add(indexX);
                }
                else
                {
                    out_vertices.Add(in_vertices[(int)i]);
                    out_uvs.Add(in_uvs[(int)i]);
                    out_normals.Add(in_normals[(int)i]);
                    out_indices.Add((uint)out_vertices.Count - 1);
                }
                //Logger.Error(index);
            }
            return new Mesh(out_vertices.ToArray(), out_normals.ToArray(), out_uvs.ToArray(), out_indices.ToArray());
        }


        public static bool getSimilarVertexindex(Vector3 in_vertex, Vector3 in_normal, Vector2 in_uv, ref List<Vector3> out_vertices, ref List<Vector3> out_normals, ref List<Vector2> uvs, ref ushort index)
        {
            for (uint i = 0; i < out_vertices.Count; i++)
            {
                if (MathUtil.Vector3ApproximatelyEqual(in_vertex, out_vertices[(ushort)i]) &&
                    MathUtil.Vector3ApproximatelyEqual(in_normal, out_normals[(ushort)i]) &&
                    MathUtil.Vector2ApproximatelyEqual(in_uv, uvs[(ushort)i]))
                {
                    index = (ushort)i;
                    return true;
                }

            }

            return false;
        }
    }
}
