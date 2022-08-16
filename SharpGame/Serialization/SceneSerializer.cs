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
            foreach (int actor in scene.EnumarateActors())
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

        public static void Deseriealize(string path, ref Scene scene)
        {
            using TextReader textReader = new StreamReader(path);
            using JsonReader reader = new JsonTextReader(textReader);

            bool readingActors = false;
            Actor currentActor = Actor.Null;
            string currentProperty = "root";
            Stack<string> properties = new Stack<string>();
            Stack<string> arrays = new Stack<string>();

            while (reader.Read())
            {
                Logger.Info($"TokenType: {reader.TokenType} Value: {reader.Value}");

                if (reader.TokenType == JsonToken.PropertyName)
                {
                    currentProperty = (string)reader.Value;
                    continue;
                }

                if (reader.TokenType == JsonToken.StartObject)
                {
                    properties.Push(currentProperty);
                }

                if (reader.TokenType == JsonToken.StartArray)
                {
                    arrays.Push(currentProperty);
                    currentProperty = "arrayItem";
                }

                if (reader.TokenType == JsonToken.EndObject)
                {
                    properties.Pop();
                }

                if (reader.TokenType == JsonToken.EndArray)
                {
                    arrays.Pop();
                }

                if (arrays.Peek() == "actors")
                {

                }
            }
        }
    }
}
