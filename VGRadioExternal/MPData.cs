using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace VGRadioExternal {

    class MPData {
        public string Description;
        [JsonProperty(Required = Required.Always)]
        public string ProcessName;

        public int MinVolume;

        public ReadVolume ReadVolume;
        [JsonProperty(Required = Required.Always)]
        public SetVolume SetVolume;

        /// <summary>
        /// Reads a music player file and checks that it has what its hook needs. Throws InvalidDataException saying what's wrong.
        /// </summary>
        public static MPData Load(string path) {
            MPData mp = ConfigFile.Read<MPData>(path);
            ConfigFile.CheckProcessName(mp.ProcessName);
            SetVolume setVolume = mp.SetVolume;
            switch (setVolume.Type) {
                case HookMethods.SendMessage:
                    RequireForHook(!string.IsNullOrEmpty(setVolume.WindowName), "windowName", setVolume.Type);
                    RequireForHook(setVolume.WMsg != null, "wMsg", setVolume.Type);
                    RequireForHook(setVolume.WParam != null, "wParam", setVolume.Type);
                    RequireForHook(setVolume.LParam != null, "lParam", setVolume.Type);
                    ConfigFile.Require(mp.ReadVolume != null || !setVolume.CarriesVolume(), "setVolume uses " + MessageParameter.VolumeText + ", which needs readVolume, to know what volume to put back.");
                    break;
                case HookMethods.ProcessStart:
                    RequireForHook(!string.IsNullOrEmpty(setVolume.Arguments), "arguments", setVolume.Type);
                    break;
                case HookMethods.KeyPress:
                    ConfigFile.Require((setVolume.Keys & Keys.KeyCode) != Keys.None, "setVolume.keys is missing, or has no key besides Shift, Ctrl and Alt.");
                    break;
            }
            return mp;
        }

        static void RequireForHook(bool present, string field, HookMethods hook) {
            ConfigFile.Require(present, "setVolume." + field + " is missing, and " + hook + " needs it.");
        }
    }

    class ReadVolume {
        public string Module;
        [JsonProperty(Required = Required.Always)]
        [JsonConverter(typeof(HexIntConverter))]
        public int AddressOffset;
        [JsonProperty(Required = Required.Always)]
        public WinApi.Types Type;
    }

    class SetVolume {
        [JsonProperty(Required = Required.Always)]
        public HookMethods Type;
        public string WindowName;
        public WinApi.wMsg? WMsg;
        public MessageParameter LParam;
        public MessageParameter WParam;
        public string Arguments;
        [JsonConverter(typeof(KeysJsonConverter))]
        public Keys Keys;

        /// <summary>
        /// Whether wParam or lParam is {volume}
        /// </summary>
        public bool CarriesVolume() {
            return (WParam?.IsVolume ?? false) || (LParam?.IsVolume ?? false);
        }
    }

    /// <summary>
    /// A wParam or lParam: a number, or {volume} for the volume being sent
    /// </summary>
    [JsonConverter(typeof(MessageParameter.Converter))]
    class MessageParameter {
        public const string VolumeText = "{volume}";

        public bool IsVolume { get; private set; }
        int _number;

        public int Value(int volume) {
            return IsVolume ? volume : _number;
        }

        internal class Converter : JsonConverter<MessageParameter> {
            public override bool CanWrite => false;

            public override MessageParameter ReadJson(JsonReader reader, Type objectType, MessageParameter existingValue, bool hasExistingValue, JsonSerializer serializer) {
                if (reader.TokenType == JsonToken.Null) {
                    return null;
                }
                if (reader.TokenType == JsonToken.String && (string)reader.Value == VolumeText) {
                    return new MessageParameter { IsVolume = true };
                }
                if (HexIntConverter.TryRead(reader, out int number)) {
                    return new MessageParameter { _number = number };
                }
                throw ConfigFile.Error(reader, "a number or \"" + VolumeText + "\"");
            }

            public override void WriteJson(JsonWriter writer, MessageParameter value, JsonSerializer serializer) {
                throw new NotSupportedException();
            }
        }
    }

    public enum HookMethods {
        SendMessage,    // for winamp
        ProcessStart,    // for foobar
        AudioSession,    // for spotify, or any other app in the volume mixer
        KeyPress    // for any player with a global mute hotkey
    }

}
