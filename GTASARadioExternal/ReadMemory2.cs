using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GTASARadioExternal
{
    class ReadMemory2
	{
        Process _musicPlayerProcess;
        int _musicPlayerWindow = 0;
        int _volume = 0;
		int _addressVolume = 0x0;
        int _addressRunning = 0x0;

        [DllImport("kernel32.dll")]
        public static extern Int32 ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress,
          [In, Out] byte[] buffer, UInt32 size, out IntPtr lpNumberOfBytesRead);

        [DllImport("user32.dll")]
        public static extern int SendMessage(int hwnd, int wMsg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern int FindWindow(string lpClassName, String lpWindowName);

        public void GetMusicPlayer()
		{
            int moduleAddress = 0;
            Process process = GetProcess(Settings.ProcessName);
            if (process == null) {
                // abort
                // TODO: do something
            }
            _musicPlayerProcess = process;

            moduleAddress = GetModuleAddress(process, Settings.VolumeModuleName);
            _addressVolume = moduleAddress + Settings.VolumeAddressOffset;

            moduleAddress = GetModuleAddress(process, Settings.RunningModuleName);
            _addressRunning = moduleAddress + Settings.RunningAddressOffset;

            // Either go with windowName (Winamp) or executableLocation (Foobar) here
            _musicPlayerWindow = FindWindow(Settings.WindowName, null);
            // TODO: do something
            // TODO: ensure the max volume bootup glitch doesn't happen
        }

        /// <summary>
        /// Set radio to on or off
        /// </summary>
        /// <param name="on">whether to turn the radio on</param>
        public void SetRadio(bool on) {
            if (on) {   // turn radio on
                SendMessage(_musicPlayerWindow, 0x0400, _volume, Settings.MessageLParam); //0x0400 = WM_USER
            }
            else {      // turn radio off
                _volume = CheckMusicRunning();
                SendMessage(_musicPlayerWindow, 0x0400, 0, Settings.MessageLParam); //0x0400 = WM_USER
            }
        }

        /// <summary>
        /// toggles the radio on or off
        /// </summary>
        public void ToggleRadio() {
            int volume = CheckMusicRunning();
            if (volume == 0) {  // turn radio on
                SendMessage(_musicPlayerWindow, 0x0400, _volume, Settings.MessageLParam); //0x0400 = WM_USER
            }
            else {              // turn radio off
                _volume = volume;
                SendMessage(_musicPlayerWindow, 0x0400, 0, Settings.MessageLParam); //0x0400 = WM_USER
            }
        }
        
        public int CheckMusicRunning() {
            int running = 0;
            try {
                running = ReadValue(_musicPlayerProcess, _addressRunning, Settings.RunningAddressType);
            }
            catch {
                // TODO: catch errors
            }
            return running;
        }
        
        public int CheckMusicVolume() {
            int volume = -1;
            try {
                volume = ReadValue(_musicPlayerProcess, _addressVolume, Settings.VolumeAddressType);
            }
            catch {
                // TODO: catch errors
            }
            return volume;
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
                // TODO: Do stuff
                return null;
            }
            else if (processes.Length > 1) {
                // Multiple instances of the player are running
                // TODO: Do stuff
                return processes[0];
            }
            else {
                // Player is running
                return processes[0];
            }
        }

        /// <summary>
        /// Gets a module address from a process by name, or the base address of the process.
        /// </summary>
        /// <param name="process">the process</param>
        /// <param name="name">the name of the module. leave blank to get the process base address instead.</param>
        /// <returns></returns>
		int GetModuleAddress(Process process, string name = "") {
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

        /// <summary>
        /// Reads a value of bytes in a process
        /// </summary>
        /// <param name="process"></param>
        /// <param name="address"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        int ReadValue(Process process, long address, Types type) {
            byte[] buffer;            
            switch (type) {
                case Types.Byte:
                    buffer = new byte[1];
                    break;
                case Types.Float:
                case Types.FourBytes:
                default:
                    buffer = new byte[4];
                    break;
            }
            ReadProcessMemory(process.Handle, new IntPtr(address), buffer, (uint)buffer.Length, out IntPtr lpNumberOfBytesRead);
            switch (type) {
                case Types.Byte:
                    return (int)buffer[0];
                case Types.Float:
                    float bufferF = BitConverter.ToSingle(buffer, 0);
                    return Convert.ToInt32(bufferF);    // in my old code it added 50 to this, but I dont know why. Is it something Foobar related?
                case Types.FourBytes:
                default:
                    return BitConverter.ToInt32(buffer, 0);
            }
        }

	}
}
