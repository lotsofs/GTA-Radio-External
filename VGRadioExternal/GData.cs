using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VGRadioExternal {
	class GData {
		public string Description;
		public string ProcessName;
		public string ModuleName;
		public RadioAddress RadioAddress;
	}

	class RadioAddress {
		public string Module;
		[JsonConverter(typeof(HexIntConverter))]
		public int AddressOffset;
		public string Type;
		public int[] OnValues;
		public int[] OffValues;
	}
}
