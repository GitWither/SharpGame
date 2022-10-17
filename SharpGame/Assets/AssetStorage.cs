using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualBasic.CompilerServices;

using Newtonsoft.Json;

namespace SharpGame.Assets
{
    public struct AssetData
    {
        public static AssetData Invalid = new AssetData(-1, "null", AssetType.Invalid);
        public bool HasLoaded { get; set; }
        public long Id { get; }
        public string Path { get; }
        public AssetType Type { get; }

        public AssetData(long id, string path, AssetType type)
        {
            this.Id = id;
            this.Path = path;
            this.HasLoaded = false;
            this.Type = type;
        }

        public bool IsValid()
        {
            return Id != -1 && Path != "null";
        }
    }

    public class AssetStorage
    {
        private readonly Random m_Random = new Random();

        private readonly Dictionary<long, Asset> m_Assets = new();
        private readonly Dictionary<Asset, long> m_AssetIds = new();
        private readonly Dictionary<long, AssetData> m_AssetRegistry = new();

        private readonly Dictionary<AssetType, IAssetSerializer> m_TypeToSerializer = new()
            {
                { AssetType.Material, new MaterialSerializer() },
                { AssetType.Mesh, new MeshSerializer()},
                { AssetType.Texture, new TextureSerializer()}
            };

    public void Initialize()
        {
            if (!File.Exists("AssetStorage.txt")) return;

            using FileStream fileStream = File.OpenRead("AssetStorage.txt");
            using StreamReader reader = new StreamReader(fileStream);

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();

                if (string.IsNullOrEmpty(line)) continue;


                string[] parts = line.Split(":", 2);


                if (!long.TryParse(parts[0], out long assetId)) continue;

                string path = parts[1];

                if (!File.Exists(path)) continue;

                m_AssetRegistry.Add(assetId, new AssetData(assetId, path, GetAssetTypeOfPath(path)));

            }
        }

        public void Shutdown()
        {
            using FileStream fileStream = File.OpenWrite("AssetStorage.txt");
            using StreamWriter writer = new StreamWriter(fileStream);

            foreach (KeyValuePair<long, AssetData> idAssetPair in m_AssetRegistry)
            {
                writer.Write(idAssetPair.Key);
                writer.Write(":");
                writer.Write(idAssetPair.Value.Path);
                writer.WriteLine();
            }

            foreach (KeyValuePair<long, Asset> idAssetPair in m_Assets)
            {
                if (m_AssetRegistry[idAssetPair.Key].HasLoaded)
                {
                    idAssetPair.Value.Dispose();
                }
            }
        }

        public long ImportAsset(string file)
        {
            AssetData data = GetAssetDataByPath(file);
            if (data.IsValid())
            {
                return data.Id;
            }

            data = new AssetData(m_Random.NextInt64(), file, GetAssetTypeOfPath(file));
            m_AssetRegistry.Add(data.Id, data);

            return data.Id;
        }

        public long GetAssetId(Asset asset)
        {
            return m_AssetIds[asset];
        }

        public T GetAsset<T>(long id) where T : Asset
        {
            if (!m_AssetRegistry.ContainsKey(id)) return null;

            AssetData data = m_AssetRegistry[id];

            if (!data.HasLoaded)
            {
                Asset asset = m_TypeToSerializer[data.Type].Deserialize(data);
                if (asset == null) return null;

                data.HasLoaded = true;
                m_AssetRegistry[id] = data;

                m_Assets.Add(id, asset);
                m_AssetIds.Add(asset, id);
                return (T)asset;
            }

            return (T)m_Assets[id];
        }

        AssetData GetAssetDataByPath(string path)
        {
            foreach (KeyValuePair<long, AssetData> idAssetDataPair in m_AssetRegistry)
            {
                if (idAssetDataPair.Value.Path == path) return idAssetDataPair.Value;
            }

            return AssetData.Invalid;
        }

        AssetType GetAssetTypeOfPath(string path)
        {
            string ext = Path.GetExtension(path);

            switch (ext)
            {
                case ".sgmat":
                    return AssetType.Material;
                case ".obj":
                    return AssetType.Mesh;
                case ".png":
                    return AssetType.Texture;
            }

            return AssetType.Invalid;
        }

        public IEnumerable<KeyValuePair<long, AssetData>> EnumerateAssets()
        {
            foreach (KeyValuePair<long, AssetData> idAssetDataPair in m_AssetRegistry)
            {
                yield return idAssetDataPair;
            }
        }
    }
}
