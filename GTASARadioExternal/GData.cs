using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTASARadioExternal {
	class GData {
		public string Description;
		public string ProcessName;
		public RadioAddresses RadioAddress;
	}

	class RadioAddresses {
		public string Module;
		public int AddressOffset;
		public string Type;
		public int[] OnValues;
		public int[] OffValues;
	}
}
