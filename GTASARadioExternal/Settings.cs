using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTASARadioExternal
{
	class Settings
	{
		public static string ProcessName = "winamp";
		
		public static string VolumeModuleName = "out_ds.dll";
		public static int VolumeAddressOffset = 0xB0A0;
        public static Types VolumeAddressType = Types.FourBytes;

        public static string RunningModuleName = string.Empty;
		public static int RunningAddressOffset = 0xBD1EC;
        public static Types RunningAddressType = Types.FourBytes;

		public static string WindowName = "Winamp v1.x";
        public static int MessageLParam = 122;

        public static int volumeLowerBounds = 0;
        public static int volumeUpperBounds = 255;
	}
}
