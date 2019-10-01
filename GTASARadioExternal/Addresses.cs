using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTASARadioExternal
{
	class Addresses
	{
		public static string ProcessName = "winamp";
		
		public static string VolumeModuleName = "out_ds.dll";
		public static int VolumeAddressOffset = 0xB0A0;

		public static string RunningModuleName = string.Empty;
		public static int RunningAddressOffset = 0xBD1EC;

		public static string WindowName = "Winamp v1.x";
	}
}
