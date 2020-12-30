using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpGame.Graphics
{
    public class Material
    {
        public Shader Shader { get; set; }
        public Texture BaseMap { get; set; }
        public Texture NormalMap { get; set; }
        public float Specularity { get; set; }

        public Material(Shader shader, Texture baseMap) : this(shader, baseMap, null, 0.0f)
        {
        }

        public Material(Shader shader, Texture baseMap, Texture normalMap) : this(shader, baseMap, baseMap, 0.0f)
        {
        }

        public Material(Shader shader, Texture baseMap, float specularity) : this(shader, baseMap, null, specularity)
        {
        }
        
        public Material(Shader shader, Texture baseMap, Texture normalMap, float specularity)
        {
            this.Shader = shader;
            this.BaseMap = baseMap;
            this.NormalMap = normalMap;
            this.Specularity = specularity;
        }
    }
}
