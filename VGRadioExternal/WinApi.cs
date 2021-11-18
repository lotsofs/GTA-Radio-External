using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace VGRadioExternal {
    static class WinApi {

        public enum wMsg {
            WM_NULL = 0x0000,
            WM_CREATE = 0x0001,
            WM_DESTROY = 0x0002,
            WM_MOVE = 0x0003,
            WM_SIZE = 0x0005,
            WM_ACTIVATE = 0x0006,
            WM_SETFOCUS = 0x0007,
            WM_KILLFOCUS = 0x0008,
            WM_ENABLE = 0x000A,
            WM_SETREDRAW = 0x000B,
            WM_SETTEXT = 0x000C,
            WM_GETTEXT = 0x000D,
            WM_GETTEXTLENGTH = 0x000E,
            WM_PAINT = 0x000F,
            WM_CLOSE = 0x0010,
            // todo: fill this in
            WM_COMMAND = 0x0111,
            WM_USER = 0x0400,
        }

        internal static Process GetProcess(object processName) {
            throw new NotImplementedException();
        }

        public enum Types {
            Float,
            Byte,
            FourBytes
        }

        [DllImport("kernel32.dll")]
        public static extern Int32 ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress,
            [In, Out] byte[] buffer, UInt32 size, out IntPtr lpNumberOfBytesRead);

        [DllImport("user32.dll")]
        public static extern int SendMessage(int hwnd, int wMsg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern int FindWindow(string lpClassName, String lpWindowName);

        [DllImport("psapi.dll")]
        public static extern bool EnumProcessModulesEx(IntPtr hProcess, IntPtr[] lphModule, int cb, out int lpcbNeeded, uint dwFilterFlag);

        [DllImport("psapi.dll")]
        public static extern uint GetModuleFileNameEx(IntPtr hProcess, IntPtr hModule, StringBuilder lpBaseName, uint nSize);

        [DllImport("psapi.dll")]
        public static extern bool GetModuleInformation(IntPtr hProcess, IntPtr hModule, out ModuleInformation lpmodinfo, uint cb);



        public const int MAX_PATH = 260;

        public struct ModuleInformation {
            public IntPtr lpBaseOfDll;
            public uint SizeOfImage;
            public IntPtr EntryPoint;
        }



        /// <summary>
        /// Reads a value of bytes in a process
        /// </summary>
        /// <param name="process"></param>
        /// <param name="address"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static int ReadValue(Process process, long address, Types type) {
            byte[] buffer;
            switch (type) {     // todo: read the amount of bytes first, and do the conversion elsewhere
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

        /// <summary>
        /// Gets a process by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Process GetProcess(string name) {
            Process[] processes = Process.GetProcessesByName(name);
            if (processes.Length == 0) {
                // Player is not running
                return null;
            }
            else if (processes.Length > 1) {
                // Multiple instances of the player are running
                // TODO: Check if the process is not null
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
		public static long GetModuleAddress(Process process, string name = "") {
            if (string.IsNullOrEmpty(name)) {
                return process.MainModule.BaseAddress.ToInt64();
            }
            //Debug.WriteLine(process.Modules.Count);
            foreach (ProcessModule pm in process.Modules) {
                if (pm.ModuleName == name) {
                    return pm.BaseAddress.ToInt64();
                }
            }
            // Module not found, are we running 64 bit? Lets try another method. TODO: Tidy this
            List<Module> w64Modules = CollectModules(process);
            foreach (Module m in w64Modules) {
                if (m.ModuleName == name) {
                    return m.BaseAddress.ToInt64();
                }
            }
            // Nothing found
            return -1;
        }






        // TODO: What happens if I run this from x86 mode?
        public static List<Module> CollectModules(Process process) {
            List<Module> modules = new List<Module>();
            
            IntPtr[] modulePointers = new IntPtr[0];
            int bytesNeeded;

            if (!EnumProcessModulesEx(process.Handle, modulePointers, 0, out bytesNeeded, 0x03)) {
                // no DLLs present
                return modules;
            }

            int modulesCount = bytesNeeded / IntPtr.Size;
            modulePointers = new IntPtr[modulesCount];

            if (EnumProcessModulesEx(process.Handle, modulePointers, bytesNeeded, out bytesNeeded, 0x03)) {
                for (int i = 0; i < modulesCount; i++) {
                    StringBuilder moduleFilePath = new StringBuilder(MAX_PATH);
                    GetModuleFileNameEx(process.Handle, modulePointers[i], moduleFilePath, (uint)moduleFilePath.Capacity);
                    
                    string moduleName = Path.GetFileName(moduleFilePath.ToString());
                    ModuleInformation moduleInformation;
                    GetModuleInformation(process.Handle, modulePointers[i], out moduleInformation, (uint)(modulePointers.Length * IntPtr.Size));

                    Module module = new Module(moduleName, moduleInformation.lpBaseOfDll, moduleInformation.SizeOfImage);
                    modules.Add(module);
                }
            }

            return modules;
        }
    }
}
