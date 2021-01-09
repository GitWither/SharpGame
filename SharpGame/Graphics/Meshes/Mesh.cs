﻿using OpenTK;
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
        public static Mesh GuiQuad = new Mesh(
            new Vector3[] {
                new Vector3(-1, 1, 0),
                new Vector3(-1, -1, 0),
                new Vector3(1, 1, 0),
                new Vector3(1, -1, 0)
            }, 
            null,
            new Vector2[] {
                new Vector2(0, 0),
                new Vector2(0, 1),
                new Vector2(1, 0),
                new Vector2(1, 1)
            }, 
            new int[] {0, 1, 2, 2, 1, 3 }
            );


        public Vector3[] Vertices { get; set; }
        public Vector2[] FaceTexCoords { get; set; }
        public Vector3[] Normals { get; set; }
        public int[] Indices { get; set; }
        public Mesh(Vector3[] vertices, Vector3[] normals, Vector2[] texCoords, int[] faceIndices)
        {
            this.Vertices = vertices;
            this.FaceTexCoords = texCoords;
            this.Indices = faceIndices;
            this.Normals = normals;
        }

        //I don't know why, I don't want to know why, I shouldn't have to wonder why, but this piece of code was the most painful thing I ever worked with
        public static Mesh FromOBJ(string file)
        {
            List<Vector3> tempVertices = new List<Vector3>();
            List<Vector2> tempUVs = new List<Vector2>();
            List<Vector3> tempNormals = new List<Vector3>();

            List<int> vertexIndices = new List<int>();
            List<int> uvIndices = new List<int>();
            List<int> normalIndices = new List<int>();

            using (StreamReader sr = new StreamReader(SharedConstants.MeshFolder + file + SharedConstants.MeshExtension))
            {
                while (sr.Peek() >= 0)
                {
                    string line = sr.ReadLine();
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

                                if (int.TryParse(vertexValues[0], out int index) &&
                                    int.TryParse(vertexValues[1], out int texIndex) &&
                                    int.TryParse(vertexValues[2], out int normalIndex))
                                {
                                    vertexIndices.Add(index);
                                    uvIndices.Add(texIndex);
                                    normalIndices.Add(normalIndex);
                                }
                            }
                            break;
                        default:
                            continue;
                    }
                }
            }

            List<Vector3> in_vertices = new List<Vector3>();
            List<Vector2> in_uvs = new List<Vector2>();
            List<Vector3> in_normals = new List<Vector3>();

            for (int i = 0; i < vertexIndices.Count; i++)
            {
                int vertexIndex = vertexIndices[i];
                int texIndex = uvIndices[i];
                int normalIndex = normalIndices[i];

                Vector3 vertex = tempVertices[vertexIndex - 1];
                Vector3 normal = tempNormals[normalIndex - 1];
                Vector2 texCoord = tempUVs[texIndex - 1];

                in_vertices.Add(vertex);
                in_normals.Add(normal);
                in_uvs.Add(texCoord);
            }

            List<Vector3> out_vertices = new List<Vector3>();
            List<Vector2> out_uvs = new List<Vector2>();
            List<Vector3> out_normals = new List<Vector3>();
            List<int> out_indices = new List<int>();
            int indexX = 0;
            for (int i = 0; i < in_vertices.Count; i++)
            {
                bool found = false;
                for (int p = 0; p < out_vertices.Count; p++)
                {
                    if (MathUtil.Vector3ApproximatelyEqual(in_vertices[i], out_vertices[p]) &&
                        MathUtil.Vector3ApproximatelyEqual(in_normals[i], out_normals[p]) &&
                        MathUtil.Vector2ApproximatelyEqual(in_uvs[i], out_uvs[p]))
                    {
                        indexX = p;
                        found = true;
                        break;
                    }

                }

                if (found)
                {
                    out_indices.Add(indexX);
                }
                else
                {
                    out_vertices.Add(in_vertices[i]);
                    out_uvs.Add(in_uvs[i]);
                    out_normals.Add(in_normals[i]);
                    out_indices.Add(out_vertices.Count - 1); 
                }
            }

            return new Mesh(out_vertices.ToArray(), out_normals.ToArray(), out_uvs.ToArray(), out_indices.ToArray());
        }

        public static Mesh FromText(string text)
        {
            Vector3[] vertices = new Vector3[text.Length * 4];
            Vector2[] uvs = new Vector2[text.Length * 4];
            int[] indices = new int[text.Length * 6];

            float size = 1f;
            int x = 0;
            int y = 0;
            int[] faceIndices = new int[] { 0, 1, 2, 2, 1, 3};
            float likeBreakFactor = 0;
            float spacingFactor = 0;
            for (int i = 0; i < text.Length * 4; i += 4)
            {
                char character = text[(int)(i * 0.25)];

                float uvX = (character % 16) / 16.0f;
                float uvY = (character / 16) / 16.0f;

                if (character == '\n')
                {
                    likeBreakFactor += y + size + 0.5f;
                    spacingFactor = -1f;
                }


                Vector3 vertexUpLeft =    new Vector3(x + spacingFactor * size,        y + size - likeBreakFactor, 0);
                Vector3 vertexUpRight =   new Vector3(x + spacingFactor * size + size, y + size - likeBreakFactor, 0);
                Vector3 vertexDownRight = new Vector3(x + spacingFactor * size + size, y - likeBreakFactor, 0);
                Vector3 vertexDownLeft =  new Vector3(x + spacingFactor * size,        y - likeBreakFactor, 0);

                vertices[i] = vertexUpLeft;
                vertices[i + 1] = vertexDownLeft;
                vertices[i + 2] = vertexUpRight;
                vertices[i + 3] = vertexDownRight;


                Vector2 uvUpLeft =    new Vector2(uvX,                uvY);
                Vector2 uvUpRight =   new Vector2(uvX + 1.0f / 16.0f, uvY);
                Vector2 uvDownRight = new Vector2(uvX + 1.0f / 16.0f, (uvY + 1.0f / 16.0f));
                Vector2 uvDownLeft =  new Vector2(uvX,                (uvY + 1.0f / 16.0f));

                uvs[i] = uvUpLeft;
                uvs[i + 1] = uvDownLeft;
                uvs[i + 2] = uvUpRight;
                uvs[i + 3] = uvDownRight;

                for (int j = 0; j < 6; j++)
                {
                    indices[i / 4 * 6 + j] = faceIndices[j] + i;
                }

                spacingFactor += 1f;
            }

            return new Mesh(vertices, new Vector3[] { }, uvs, indices);
        }
    }
}
