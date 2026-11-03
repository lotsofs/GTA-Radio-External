using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace VGRadioExternal {
    /// <summary>
    /// Reads game and music player files, and says what's wrong with one that can't be used
    /// </summary>
    static class ConfigFile {
        static readonly JsonSerializerSettings Settings = new JsonSerializerSettings {
            // errors spell fields the way the files do. Reading still ignores case
            ContractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() },
            Converters = { new EnumNameConverter() }
        };

        /// <summary>
        /// Reads a file into T. Throws InvalidDataException saying what's wrong, if it can't be read or a value has the wrong form.
        /// </summary>
        public static T Read<T>(string path) where T : class {
            T config;
            try {
                config = JsonConvert.DeserializeObject<T>(File.ReadAllText(path), Settings);
            }
            catch (Exception ex) when (ex is JsonException || ex is IOException || ex is UnauthorizedAccessException) {
                throw new InvalidDataException(ex.Message, ex);
            }
            Require(config != null, "The file is empty.");
            return config;
        }

        /// <summary>
        /// Throws InvalidDataException with the problem, unless the condition holds
        /// </summary>
        public static void Require(bool condition, string problem) {
            if (!condition) {
                throw new InvalidDataException(problem);
            }
        }

        public static void CheckProcessName(string processName) {
            Require(processName.Length > 0, "processName is empty.");
            // processes are looked up without the extension, so this would never be found
            Require(!processName.EndsWith(".exe", StringComparison.OrdinalIgnoreCase), "processName is the name without .exe, such as " + Path.GetFileNameWithoutExtension(processName) + ".");
        }

        /// <summary>
        /// The error for a value a converter can't read, saying where it is and what it should be
        /// </summary>
        /// <param name="expected">what the value should be, such as "a number"</param>
        public static JsonSerializationException Error(JsonReader reader, string expected) {
            string value;
            switch (reader.TokenType) {
                case JsonToken.String:
                    value = "\"" + reader.Value + "\"";
                    break;
                case JsonToken.StartObject:
                    value = "an object";
                    break;
                case JsonToken.StartArray:
                    value = "a list";
                    break;
                case JsonToken.Null:
                    value = "null";
                    break;
                default:
                    value = Convert.ToString(reader.Value);
                    break;
            }
            return new JsonSerializationException(reader.Path + " must be " + expected + ", not " + value + ".");
        }
    }

    /// <summary>
    /// Reads an enum from one of its names, in any case. Numbers aren't read, because Newtonsoft would take any number, even one the enum doesn't have.
    /// </summary>
    class EnumNameConverter : JsonConverter {
        public override bool CanWrite => false;

        public override bool CanConvert(Type objectType) {
            return (Nullable.GetUnderlyingType(objectType) ?? objectType).IsEnum;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
            Type enumType = Nullable.GetUnderlyingType(objectType) ?? objectType;
            if (reader.TokenType == JsonToken.Null && enumType != objectType) {
                return null;
            }
            string[] names = Enum.GetNames(enumType);
            string name = reader.TokenType == JsonToken.String
                ? names.FirstOrDefault(n => n.Equals((string)reader.Value, StringComparison.OrdinalIgnoreCase))
                : null;
            if (name == null) {
                throw ConfigFile.Error(reader, string.Join(", ", names.Take(names.Length - 1)) + " or " + names.Last());
            }
            return Enum.Parse(enumType, name);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
            throw new NotSupportedException();
        }
    }

    /// <summary>
    /// Reads keys the way KeysConverter writes them, such as Ctrl+Shift+P
    /// </summary>
    class KeysJsonConverter : JsonConverter<Keys> {
        const string Expected = "keys such as Ctrl+Shift+P or F13";

        public override bool CanWrite => false;

        public override Keys ReadJson(JsonReader reader, Type objectType, Keys existingValue, bool hasExistingValue, JsonSerializer serializer) {
            if (reader.TokenType != JsonToken.String) {
                throw ConfigFile.Error(reader, Expected);
            }
            try {
                // an empty string reads as null, which is no key
                return (Keys?)new KeysConverter().ConvertFromInvariantString((string)reader.Value) ?? Keys.None;
            }
            catch (Exception ex) when (ex is ArgumentException || ex is FormatException) {
                throw ConfigFile.Error(reader, Expected);
            }
        }

        public override void WriteJson(JsonWriter writer, Keys value, JsonSerializer serializer) {
            throw new NotSupportedException();
        }
    }

    /// <summary>
    /// Reads a name, or a list of names, as a list
    /// </summary>
    class NameListConverter : JsonConverter<string[]> {
        public override bool CanWrite => false;

        public override string[] ReadJson(JsonReader reader, Type objectType, string[] existingValue, bool hasExistingValue, JsonSerializer serializer) {
            if (reader.TokenType == JsonToken.String) {
                return new[] { (string)reader.Value };
            }
            if (reader.TokenType != JsonToken.StartArray) {
                throw ConfigFile.Error(reader, "a name or a list of names");
            }
            List<string> names = new List<string>();
            while (reader.Read() && reader.TokenType != JsonToken.EndArray) {
                if (reader.TokenType == JsonToken.Comment) {
                    continue;
                }
                if (reader.TokenType != JsonToken.String) {
                    throw ConfigFile.Error(reader, "a name");
                }
                names.Add((string)reader.Value);
            }
            return names.ToArray();
        }

        public override void WriteJson(JsonWriter writer, string[] value, JsonSerializer serializer) {
            throw new NotSupportedException();
        }
    }
}
