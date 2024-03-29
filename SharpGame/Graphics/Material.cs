﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using SharpGame.Assets;
using SharpGame.Graphics.Vaos;
using SharpGame.Util;

namespace SharpGame.Graphics
{
    public class Material : Asset
    {
        public Shader Shader { get; set; }
        public Texture BaseMap { get; set; }
        public Texture NormalMap { get; set; }
        public Texture EmissionMap { get; set; }
        public Vector3 AmbientColor { get; set; }
        public Vector3 DiffuseColor { get; set; }


        public Material(Shader shader, Texture baseMap) : this(shader, baseMap, null, null, 0.0f)
        {
        }

        public Material(Shader shader, Texture baseMap, Texture normalMap) : this(shader, baseMap, normalMap, null, 0.0f)
        {
        }

        public Material(Shader shader, Texture baseMap, Texture normalMap, Texture emissionMap) : this(shader, baseMap, normalMap, emissionMap, 0.0f)
        {

        }

        public Material(Shader shader, Texture baseMap, float specularity) : this(shader, baseMap, null, null, specularity)
        {
        }
        
        public Material(Shader shader, Texture baseMap, Texture normalMap, Texture emissionMap, float specularity)
        {
            this.Shader = shader;
            this.BaseMap = baseMap;
            this.NormalMap = normalMap;
            this.EmissionMap = emissionMap;
        }


        public override void Dispose()
        {
            Shader?.Dispose();
        }
    }
}
