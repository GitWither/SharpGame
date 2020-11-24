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
    public class Mesh : IDisposable
    {
        private VertexArrayObject vao;
        public Vector3[] Vertices
        {
            get
            {
                return this.vao.Vertices;
            }
        }
        public Vector2[] FaceTexCoords
        {
            get
            {
                return this.vao.TexCoords;
            }
        }
        public uint[] FaceIndices
        {
            get
            {
                return this.vao.Indices;
            }
        }
        public Mesh(Vector3[] vertices, Vector3[] normals, Vector2[] texCoords, uint[] faceIndices)
        {
            vao = new VertexArrayObject();

            vao.AddVertices(vertices);
            vao.AddTexCoords(texCoords);
            vao.AddNormals(normals);
            vao.AddIndices(faceIndices);

            vao.Upload();
        }

        public void Clear()
        {
            vao.Clear();
        }

        public void Draw()
        {
            vao.Draw();
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

            foreach (string line in File.ReadLines(file + SharedConstants.MeshExtension))
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
                            tempUVs.Add(new Vector2(u, v));
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

            List<Vector3> newVertices = new List<Vector3>();
            List<Vector2> newUVs = new List<Vector2>();
            List<Vector3> newNormals = new List<Vector3>();

            for (int i = 0; i < vertexIndices.Count; i++)
            {
                uint vertexIndex = vertexIndices[i];
                uint texIndex = uvIndices[i];
                uint normalIndex = normalIndices[i];

                Vector3 vertex = tempVertices[(int)(vertexIndex - 1)];
                Vector3 normal = tempNormals[(int)(normalIndex - 1)];
                Vector2 texCoord = tempUVs[(int)(texIndex - 1)];
                newVertices.Add(vertex);
                newNormals.Add(normal);
                newUVs.Add(texCoord);
            }

            List<Vector3> finalVertices = new List<Vector3>();
            List<Vector2> finalUvs = new List<Vector2>();
            List<Vector3> finalNormals = new List<Vector3>();
            List<uint> finalIndices = new List<uint>();
            for(int i = 0; i < newVertices.Count; i++)
            {
                int index = 0;
                bool yes = test2(newVertices[i], newNormals[i], newUVs[i], ref finalVertices, ref finalNormals, ref finalUvs, ref index);
                if (yes)
                {
                    finalIndices.Add((uint)index);
                }
                else
                {
                    finalVertices.Add(newVertices[i]);
                    finalUvs.Add(newUVs[i]);
                    finalNormals.Add(newNormals[i]);
                    finalIndices.Add((uint)finalVertices.Count - 1);
                }
                Logger.Error(index);
            }
            return new Mesh(finalVertices.ToArray(), finalNormals.ToArray(), finalUvs.ToArray(), finalIndices.ToArray());
        }

        public static bool test(float x, float y)
        {
            //Logger.Debug(Math.Abs(x - y) < 0.01f);
            return Math.Abs(x - y) < 0.01f;
        }

        public static bool test2(Vector3 vertex, Vector3 normal, Vector2 uv, ref List<Vector3> vertices, ref List<Vector3> normals, ref List<Vector2> uvs, ref int index)
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                //Logger.Error(i);
                //Logger.Debug(test(vertex.X, vertices[i].X) + " " + test(vertex.Y, vertices[i].Y) + " " + test(vertex.Z, vertices[i].Z) + "/ " + test(uv.X, uvs[i].X) + " " + test(uv.Y, uvs[i].Y));
                if (test(vertex.X, vertices[i].X) &&
                    test(vertex.Y, vertices[i].Y) &&
                    test(vertex.Z, vertices[i].Z) &&
                    test(normal.X, normals[i].X) &&
                    test(normal.Y, normals[i].Y) &&
                    test(normal.Z, normals[i].Z) &&
                    test(uv.X, uvs[i].X) &&
                    test(uv.Y, uvs[i].Y))
                {
                    index = i;
                    return true;
                }

            }

            return false;
        }

        public void Dispose()
        {
            vao.Dispose();
        }
    }
}
