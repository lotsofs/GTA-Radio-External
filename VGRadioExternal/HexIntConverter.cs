using System;
using System.Globalization;
using Newtonsoft.Json;

namespace VGRadioExternal {
    /// <summary>
    /// Reads an int from a JSON number or string. Standard JSON has no hex numbers, so a string is how a config writes one, such as "0x1BE9E8".
    /// </summary>
    class HexIntConverter : JsonConverter<int> {
        public override bool CanWrite => false;

        public override int ReadJson(JsonReader reader, Type objectType, int existingValue, bool hasExistingValue, JsonSerializer serializer) {
            if (TryRead(reader, out int value)) {
                return value;
            }
            throw ConfigFile.Error(reader, "a number, such as 1234 or \"0x4D2\"");
        }

        /// <summary>
        /// Reads an int from the number or string the reader is on. Returns false if it's something else, or too big.
        /// </summary>
        public static bool TryRead(JsonReader reader, out int value) {
            value = 0;
            switch (reader.TokenType) {
                case JsonToken.Integer:
                    // a number too big for a long arrives as a BigInteger
                    if (reader.Value is long number && number >= int.MinValue && number <= int.MaxValue) {
                        value = (int)number;
                        return true;
                    }
                    return false;
                case JsonToken.String:
                    string text = (string)reader.Value;
                    if (text.StartsWith("0x", StringComparison.OrdinalIgnoreCase)) {
                        return int.TryParse(text.Substring(2), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out value);
                    }
                    return int.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out value);
                default:
                    return false;
            }
        }

        public override void WriteJson(JsonWriter writer, int value, JsonSerializer serializer) {
            throw new NotSupportedException();
        }
    }
}
