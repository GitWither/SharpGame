using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SharpGame.Graphics;
using SharpGame.Graphics.Meshes;

namespace SharpGame.Assets
{
    internal interface IAssetSerializer
    {
        public void Serialize(AssetData data, Asset asset);
        public Asset Deserialize(AssetData data);

    }

    internal class TextureSerializer : IAssetSerializer
    {
        public void Serialize(AssetData data, Asset asset)
        {
            throw new NotImplementedException();
        }

        public Asset Deserialize(AssetData data)
        {
            return new Texture(data.Path);
        }
    }

    internal class MeshSerializer : IAssetSerializer
    {
        public void Serialize(AssetData data, Asset asset)
        {
            throw new NotImplementedException();
        }

        public Asset Deserialize(AssetData data)
        {
            return Mesh.FromOBJ(data.Path);
        }
    }

    internal class MaterialSerializer : IAssetSerializer
    {
        public void Serialize(AssetData data, Asset asset)
        {
            using TextWriter textWriter = new StreamWriter(data.Path);
            using JsonWriter writer = new JsonTextWriter(textWriter);

            Material material = (Material)asset;

            writer.WriteStartObject();

            writer.WritePropertyName("shader");
            writer.WriteValue(material.Shader.Name);
            writer.WritePropertyName("albedo");
            writer.WriteValue(SharpGameWindow.Instance.AssetStorage.GetAssetId(material.BaseMap));

            writer.WriteEndObject();
        }

        public Asset Deserialize(AssetData data)
        {
            //TODO: Potentially convert this to faster method? 

            using TextReader textReader = new StreamReader(data.Path);
            using JsonTextReader reader = new JsonTextReader(textReader);

            JObject obj = JToken.ReadFrom(reader) as JObject;

            if (obj == null) return null;

            //string shaderType = obj.GetValue("shader")?.Value<string>();

            long albedoMap = obj.GetValue("albedo")?.Value<long>() ?? -1;

            if (albedoMap != -1)
            {
                return new Material(Shader.Unlit, SharpGameWindow.Instance.AssetStorage.GetAsset<Texture>(albedoMap));
            }

            return null;
        }
    }
}
