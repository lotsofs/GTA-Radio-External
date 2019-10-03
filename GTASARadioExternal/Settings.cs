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
        public static WinApi.Types VolumeAddressType = WinApi.Types.FourBytes;

        [Obsolete] public static string RunningModuleName = string.Empty;
        [Obsolete] public static int RunningAddressOffset = 0xBD1EC;
        [Obsolete] public static WinApi.Types RunningAddressType = WinApi.Types.FourBytes;

		public static string WindowName = "Winamp v1.x";
        public static int MessageLParam = 122;

        public static string ProcessArguments = "/command:mute"; // for foobar

        public static int volumeLowerBounds = 0;
        public static int volumeUpperBounds = 255;
	}
}
