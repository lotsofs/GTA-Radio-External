using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VGRadioExternal {
	class GData {
		public string Description;
		[JsonProperty(Required = Required.Always)]
		public string ProcessName;
		public string ModuleName;
		[JsonProperty(Required = Required.Always)]
		public RadioAddress RadioAddress;

		/// <summary>
		/// Reads a game file and checks it. Throws InvalidDataException saying what's wrong.
		/// </summary>
		public static GData Load(string path) {
			GData game = ConfigFile.Read<GData>(path);
			ConfigFile.CheckProcessName(game.ProcessName);
			// the two lists are all that says what the radio's value means
			ConfigFile.Require(game.RadioAddress.OnValues.Length > 0, "radioAddress.onValues is empty.");
			ConfigFile.Require(game.RadioAddress.OffValues.Length > 0, "radioAddress.offValues is empty.");
			return game;
		}
	}

	class RadioAddress {
		public string Module;
		[JsonProperty(Required = Required.Always)]
		[JsonConverter(typeof(HexIntConverter))]
		public int AddressOffset;
		[JsonProperty(Required = Required.Always)]
		public WinApi.Types Type;
		[JsonProperty(Required = Required.Always)]
		public int[] OnValues;
		[JsonProperty(Required = Required.Always)]
		public int[] OffValues;
	}
}
