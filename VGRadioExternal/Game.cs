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

        public Game(GData gameData) {
            _gameData = gameData;
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
                Process process = _gameData.ProcessNames.Select(name => WinApi.GetProcess(name)).FirstOrDefault(p => p != null && !p.HasExited);
                if (process == null) {
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
                if (!IsVersion(process, moduleAddress)) {
                    // the radio's address would hold something else. Not keeping the process checks it again next poll, because a game can decrypt its code after it starts
                    _process = null;
                    Status = "Not the version this file is for";
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

        /// <summary>
        /// Whether the process matches one of the file's signatures, or the file has none
        /// </summary>
        bool IsVersion(Process process, long moduleAddress) {
            if (_gameData.Signatures == null) {
                return true;
            }
            foreach (Signature signature in _gameData.Signatures) {
                try {
                    if (WinApi.ReadValue(process, moduleAddress + signature.AddressOffset, signature.Type) == signature.Value) {
                        return true;
                    }
                }
                catch (Win32Exception) {
                    // nothing at that address, which is what another version can give
                }
            }
            return false;
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

        /// <summary>
        /// Reads whether the radio is on. Returns null when it can't tell, because the game has gone, the read failed, or the value is in neither list. The status says which.
        /// </summary>
        public bool? IsRadioOn() {
            if (!IsRunning()) {
                return null;
            }

            int radio;
            try {
                radio = WinApi.ReadValue(_process, _addressRadio, _gameData.RadioAddress.Type);
            }
            catch (Exception ex) {
                Status = "Can't read the radio: " + ex.Message;
                return null;
            }
            if (_gameData.RadioAddress.OnValues.Contains(radio)) {
                Status = "Radio on";
                return true;
            }
            if (_gameData.RadioAddress.OffValues.Contains(radio)) {
                Status = "Radio off";
                return false;
            }
            Status = "Unknown radio value " + radio;
            return null;
        }
    }
}
