using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTASARadioExternal
{
	class ReadMemory2
	{
		Process[] _musicPlayerProcesses;
		int _addressVolume = 0x0;
        int _addressRunning = 0x0;
  
		public void GetMusicPlayer()
		{
            int moduleAddress = 0;
            Process process = GetProcess(Addresses.ProcessName);
            if (process == null) {
                // abort
                // do something
            }

            moduleAddress = GetModuleAddress(process, Addresses.VolumeModuleName);
            _addressVolume = moduleAddress + Addresses.VolumeAddressOffset;

            moduleAddress = GetModuleAddress(process, Addresses.RunningModuleName);
            _addressRunning = moduleAddress + Addresses.RunningAddressOffset;

            // Either go with windowName (Winamp) or executableLocation (Foobar) here
            // do something
        }



        /// <summary>
        /// Gets a process by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Process GetProcess(string name) {
            Process[] processes = Process.GetProcessesByName(name);
            if (processes.Length == 0) {
                // Player is not running
                // Do stuff
                return null;
            }
            else if (processes.Length > 1) {
                // Multiple instances of the player are running
                // Do stuff
                return processes[0];
            }
            else {
                // Player is running
                return processes[0];
            }
        }

        /// <summary>
        /// Gets a module address from a process by name
        /// </summary>
        /// <param name="process"></param>
        /// <param name="name"></param>
        /// <returns></returns>
		int GetModuleAddress(Process process, string name) {
			if (string.IsNullOrEmpty(name)) {
				return process.MainModule.BaseAddress.ToInt32();
			}			
			foreach (ProcessModule pm in process.Modules) {
				if (pm.ModuleName == name) {
					return pm.BaseAddress.ToInt32();
				}
			}
			// throw an error here, because there's no such module name
			return 0;
		}

	}
}
