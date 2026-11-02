using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VGRadioExternal {
    class Game {
        GData _gameData;
        
        Process _process;
        int _window = 0;
        int _volume = 0;
        long _addressRadio = 0x0;
        long _addressRunning = 0x0;

        public string Status { get; private set; }

        //public static string PROCESSNAME = "GTAIV";
        //public static string MODULENAME = "";
        //public static int ADDRESSOFFSET = 0xE8463C;
        //public static WinApi.Types ADDRESSTYPE = WinApi.Types.FourBytes;
        //public List<int> LISTONVALUES = new List<int> { 2 };
        //public List<int> LISTOFFVALUES = new List<int> { 0, 1, 4 };

        public Game(string json) {
            _gameData = JsonConvert.DeserializeObject<GData>(json);
            GetProcess();
            //Configure();
            //GetProcess();
        }
        
        /// <summary>
         /// Reinitializes the game: Finds a process for the game and configures it if found. Returns whether it found something or not.
         /// </summary>
         /// <returns></returns>
        public bool GetProcess() {
            try {
                Process process = WinApi.GetProcess(_gameData.ProcessName);
                if (process == null || process.HasExited) {
                    _process = null;
                    Status = "Not running";
                    return false;
                }
                _process = process;

                long moduleAddress = WinApi.GetModuleAddress(process, _gameData.ModuleName);
                if (moduleAddress == -1) {
                    // requested module not found. This can happen while the process boots, or if it's not configured properly.
                    _process = null;
                    Status = "Missing " + _gameData.ModuleName;
                    return false;
                }
                _addressRadio = moduleAddress + _gameData.RadioAddress.AddressOffset;
                Status = "Running";
                return true;
            }
            catch (Exception ex) {
                // for example access denied, if the process runs elevated
                _process = null;
                Status = "Error: " + ex.Message;
                return false;
            }
        }

        public bool IsRunning() {
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
            if (!IsRunning()) {
                //throw new WarningException("No game is running.");
                return false;
            }

            int radio = -1;
            try {
                WinApi.Types radioValueType = (WinApi.Types)Enum.Parse(typeof(WinApi.Types), _gameData.RadioAddress.Type);
                radio = WinApi.ReadValue(_process, _addressRadio, radioValueType);
            }
            catch {
                // a failed read leaves radio at -1
            }
            if (_gameData.RadioAddress.OnValues.Contains(radio)) {
                Status = "Radio on";
                return true;
            }
            Status = "Radio off";
            if (_gameData.RadioAddress.OffValues.Contains(radio)) {
                return false;
            }
            else {
                return false;
                //throw new WarningException("Game radio address was an unrecognized value");
            }
        }
    }
}
