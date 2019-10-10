using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTASARadioExternal {
    class Game {
        Process _process;
        int _window = 0;
        int _volume = 0;
        int _addressRadio = 0x0;
        int _addressRunning = 0x0;

        public static string PROCESSNAME = "gta_sa";
        public static string MODULENAME = "";
        public static int ADDRESSOFFSET = 0x4CB760;
        public static WinApi.Types ADDRESSTYPE = WinApi.Types.FourBytes;    // TODO: This doesnt go in winapi.
        public List<int> LISTONVALUES = new List<int> { 2 };
        public List<int> LISTOFFVALUES = new List<int> { 7 };

        public Game() {
            Configure();
            GetProcess();
        }

        public void Configure() {
            // TODO: move settings file stuff here
        }
        
        /// <summary>
         /// Reinitializes the game: Finds a process for the game and configures it if found. Returns whether it found something or not.
         /// </summary>
         /// <returns></returns>
        public bool GetProcess() {
            Process process = WinApi.GetProcess(PROCESSNAME);
            if (process == null || process.HasExited) {
                _process = null;
                return false;
            }
            _process = process;

            int moduleAddress = WinApi.GetModuleAddress(process, MODULENAME);
            if (moduleAddress == -1) {
                // requested module not found. This can happen while the process boots, or if it's not configured properly. TODO: Message
                _process = null;
                return false;
            }
            _addressRadio = moduleAddress + ADDRESSOFFSET;
            return true;
        }

        public bool Running() {
            if (_process != null) {
                // no process found
                _process.Refresh();
            }
            else {
                return GetProcess();
            }
            
            if (_process.HasExited) {
                // processes exited
                return GetProcess();
            }
            return true;
        }

        public bool IsRadioOn() {
            if (!Running()) {
                //throw new WarningException("No game is running.");      // move htis to Running() TODO:
                return false;
            }

            int radio = -1;
            try {
                radio = WinApi.ReadValue(_process, _addressRadio, ADDRESSTYPE);
            }
            catch {
                // TODO: catch errors
            }
            if (LISTONVALUES.Contains(radio)) {
                return true;
            }
            if (LISTOFFVALUES.Contains(radio)) {
                return false;
            }
            else {
                return false;
                //throw new WarningException("Game radio address was an unrecognized value");
            }
        }
    }
}
