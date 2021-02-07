using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpGame.Graphics
{
    public class SkyboxMaterial
    {
        public Cubemap Cubemap { get; set; }
        public Shader Shader { get; set; }

        public SkyboxMaterial(Cubemap cubemap, Shader shader)
        {
            this.Cubemap = cubemap;
            this.Shader = shader;
        }
    }
}
