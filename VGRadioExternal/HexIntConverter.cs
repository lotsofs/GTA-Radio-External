using System;
using Newtonsoft.Json;

namespace VGRadioExternal {
    /// <summary>
    /// Reads an int from a JSON number or string. Standard JSON has no hex numbers, so a string is how a config writes one, such as "0x1BE9E8".
    /// </summary>
    class HexIntConverter : JsonConverter<int> {
        public override bool CanWrite => false;

        public override int ReadJson(JsonReader reader, Type objectType, int existingValue, bool hasExistingValue, JsonSerializer serializer) {
            switch (reader.TokenType) {
                case JsonToken.Integer:
                    return Convert.ToInt32(reader.Value);
                case JsonToken.String:
                    string text = (string)reader.Value;
                    if (text.StartsWith("0x", StringComparison.OrdinalIgnoreCase)) {
                        return Convert.ToInt32(text, 16);
                    }
                    return int.Parse(text);
                default:
                    throw new JsonSerializationException("Expected a number, not " + reader.TokenType);
            }
        }

        public override void WriteJson(JsonWriter writer, int value, JsonSerializer serializer) {
            throw new NotSupportedException();
        }
    }
}
