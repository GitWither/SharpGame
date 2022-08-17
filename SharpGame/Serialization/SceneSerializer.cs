using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenTK.Mathematics;
using SharpGame.Graphics;
using SharpGame.Graphics.Meshes;
using SharpGame.Objects;
using SharpGame.Objects.Components;
using SharpGame.Util;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace SharpGame.Serialization
{
    public static class SceneSerializer
    {
        public static void Serialize(Scene scene, string path)
        {
            using TextWriter textWriter = new StreamWriter(path);
            using JsonWriter writer = new JsonTextWriter(textWriter);

            writer.WriteStartObject();

            writer.WritePropertyName("actors");
            writer.WriteStartArray();
            foreach (int actor in scene.EnumerateActors())
            {
                Actor actorObj = new Actor(actor, scene);

                writer.WriteStartObject();

                writer.WritePropertyName("id");
                writer.WriteValue(actor);

                SerializeComponent(actorObj, writer, (NameComponent name) =>
                {
                    writer.WritePropertyName("name");
                    writer.WriteValue(name.Name);
                });

                SerializeComponent(actorObj, writer, (TransformComponent transform) =>
                {
                    writer.WritePropertyName("position");
                    writer.WriteStartArray();
                    writer.WriteValue(transform.Position.X);
                    writer.WriteValue(transform.Position.Y);
                    writer.WriteValue(transform.Position.Z);
                    writer.WriteEndArray();

                    writer.WritePropertyName("rotation");
                    writer.WriteStartArray();
                    writer.WriteValue(transform.Rotation.X);
                    writer.WriteValue(transform.Rotation.Y);
                    writer.WriteValue(transform.Rotation.Z);
                    writer.WriteEndArray();

                    writer.WritePropertyName("scale");
                    writer.WriteStartArray();
                    writer.WriteValue(transform.Scale.X);
                    writer.WriteValue(transform.Scale.Y);
                    writer.WriteValue(transform.Scale.Z);
                    writer.WriteEndArray();
                });

                SerializeComponent(actorObj, writer, (MeshComponent mesh) =>
                {
                    writer.WritePropertyName("mesh");
                    writer.WriteValue("nope");

                    writer.WritePropertyName("material");
                    writer.WriteValue("nope");
                });



                writer.WriteEndObject();
            }

            writer.WriteEndObject();
        }

        private static void SerializeComponent<T>(Actor actor, JsonWriter writer, Action<T> serializeAction) where T : struct
        {
            if (actor.HasComponent<T>())
            {
                T component = actor.GetComponent<T>();

                writer.WritePropertyName(typeof(T).Name);
                writer.WriteStartObject();

                serializeAction.Invoke(component);

                writer.WriteEndObject();
            }
        }

        public static void Deserialize(string path, ref Scene scene)
        {
            using TextReader textReader = new StreamReader(path);
            using JsonTextReader reader = new JsonTextReader(textReader);
            JsonSerializer serializer = new JsonSerializer();

            string lastPropertyName = string.Empty;
            string currentObjectName = string.Empty;
            bool readingActors = false;
            Actor currentActor = Actor.Null;


            if (reader.Read() && reader.TokenType != JsonToken.StartObject)
            {
                Logger.Error($"Scene files must be JSON objects, found: {reader.TokenType}");
                return;
            }

            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.StartObject)
                {
                    currentObjectName = lastPropertyName;
                }

                if (reader.TokenType == JsonToken.EndObject)
                {
                    currentObjectName = string.Empty;
                }

                if (reader.TokenType != JsonToken.PropertyName) continue;


                if ((string)reader.Value == "actors")
                {
                    readingActors = true;
                }

                if (readingActors)
                {
                    if ((string)reader.Value == "id")
                    {
                        int id = reader.ReadAsInt32().GetValueOrDefault(-1);
                        currentActor = scene.CreateActor("actor");
                        continue;
                    }

                    switch (currentObjectName)
                    {
                        case "NameComponent" when (string)reader.Value == "name":
                        {
                            ref NameComponent name = ref currentActor.GetComponent<NameComponent>();
                            name.Name = reader.ReadAsString();
                            break;
                        }
                        case "TransformComponent":
                        {
                            string currentPropName = (string)reader.Value;
                            ref TransformComponent transform = ref currentActor.GetComponent<TransformComponent>();
                            reader.Read();
                            float[] values = serializer.Deserialize<float[]>(reader);

                            switch (currentPropName)
                            {
                                case "position":
                                    transform.Position = new Vector3(values[0], values[1], values[2]);
                                    continue;
                                case "rotation":
                                    transform.Rotation = new Vector3(values[0], values[1], values[2]);
                                    continue;
                                case "scale":
                                    transform.Scale = new Vector3(values[0], values[1], values[2]);
                                    continue;
                            }

                            reader.Read();

                            break;
                        }
                        case "MeshComponent":
                        {
                            if (currentActor.HasComponent<MeshComponent>()) continue;

                            currentActor.AddComponent(new MeshComponent(Mesh.SkyBox,
                                new Material(Shader.Unlit, new Texture("buffalo"))));
                            //string mesh = reader.ReadAsString();
                            Logger.Info($"Mesh: {(int)currentActor}");
                            break;
                        }
                    }
                }

                lastPropertyName = (string)reader.Value;

            }
        }
    }
}
