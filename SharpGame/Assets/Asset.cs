using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpGame.Graphics;
using SharpGame.Graphics.Meshes;

namespace SharpGame.Assets
{
    public abstract class Asset
    {
        public static Asset Load(AssetData data)
        {
            //change this

            switch (data.Type)
            {
                case AssetType.Mesh:
                    return Mesh.FromOBJ(data.Path);
                case AssetType.Texture:
                    return new Texture(data.Path);
            }

            return null;
        }
    }
}
