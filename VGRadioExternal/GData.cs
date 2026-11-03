using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VGRadioExternal {
	class GData {
		public string Description;
		// an exe that goes by several names has them all here, and the first that's running is used
		[JsonProperty("processName", Required = Required.Always)]
		[JsonConverter(typeof(NameListConverter))]
		public string[] ProcessNames;
		public string ModuleName;
		// the versions the file is for, any one of which has to match. Null for any version
		public Signature[] Signatures;
		[JsonProperty(Required = Required.Always)]
		public RadioAddress RadioAddress;

		/// <summary>
		/// Reads a game file and checks it. Throws InvalidDataException saying what's wrong.
		/// </summary>
		public static GData Load(string path) {
			GData game = ConfigFile.Read<GData>(path);
			ConfigFile.Require(game.ProcessNames.Length > 0, "processName is empty.");
			foreach (string processName in game.ProcessNames) {
				ConfigFile.CheckProcessName(processName);
			}
			// an empty list would match no version, and an empty entry would fail in the poll
			ConfigFile.Require(game.Signatures == null || game.Signatures.Length > 0, "signatures is empty. Leave it out to use the file with any version.");
			ConfigFile.Require(game.Signatures == null || !game.Signatures.Contains(null), "signatures has an empty entry.");
			// the two lists are all that says what the radio's value means
			ConfigFile.Require(game.RadioAddress.OnValues.Length > 0, "radioAddress.onValues is empty.");
			ConfigFile.Require(game.RadioAddress.OffValues.Length > 0, "radioAddress.offValues is empty.");
			return game;
		}
	}

	/// <summary>
	/// A value one version of the game has at a fixed address, relative to the same module as the radio
	/// </summary>
	class Signature {
		[JsonProperty(Required = Required.Always)]
		[JsonConverter(typeof(HexIntConverter))]
		public int AddressOffset;
		[JsonProperty(Required = Required.Always)]
		public WinApi.Types Type;
		[JsonProperty(Required = Required.Always)]
		[JsonConverter(typeof(HexIntConverter))]
		public int Value;
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
