using S.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


namespace GTASARadioExternal
{
    public class MusicPlayer
	{
        public enum HookMethods {
            None,
            SendMessage,    // for winamp, spotify
            ProcessStart,    // for foobar
            MemoryWrite     // for anything else?
        }

        Process _process;
		int _addressVolume;
        int _volume;
        
        int _window;
        ProcessStartInfo _processStartInfo;

        //settings
        string _processName;

        bool _useVolume;
        string _volumeModuleName;
        int _volumeAddressOffset;
        WinApi.Types _volumeValueType;
        int _minVolume;

        HookMethods _hookMethod;
        
        int _wMsg;
        string _windowName;
        int _lParam;

        string _executableFile;
        string _arguments;

        public MusicPlayer(JsonObject json) {
            Configure(json);
            GetProcess();
        }

        public void Configure(JsonObject json) {
            // todo: clean up this mess
            JsonObject hookObject = json.GetValue("hook");
            
            _processName = json.GetValue("processName");
            
            string hook = hookObject.GetValue("type");
            _hookMethod = (HookMethods)Enum.Parse(typeof(HookMethods), hook);
            
            _useVolume = json.ContainsKey("volume");
            if (_useVolume) {
                JsonObject volumeObject = json.GetValue("volume");
                string offset = volumeObject.GetValue("addressOffset");
                string valueType = volumeObject.GetValue("type");
            
                _volumeModuleName = volumeObject.GetValue("module");
                _volumeAddressOffset = Convert.ToInt32(offset, 16);
                _volumeValueType = (WinApi.Types)Enum.Parse(typeof(WinApi.Types), valueType);
                _minVolume = (int)volumeObject.GetValue("minVolume");
            }

            switch (_hookMethod) {
                case (HookMethods.SendMessage):
                    string wMsg = hookObject.GetValue("wMsg");
                    _wMsg = (int)Enum.Parse(typeof(WinApi.wMsg), wMsg);
                    _windowName = hookObject.GetValue("windowName");
                    _lParam = (int)hookObject.GetValue("lParam");
                    break;
                case (HookMethods.ProcessStart):
                    _arguments = hookObject.GetValue("arguments");
                    break;
                default:
                    break;
            }

        }

        /// <summary>
        /// Reinitializes the music player: Finds a process for the music player and configures it if found. Returns whether it found something or not.
        /// </summary>
        /// <returns></returns>
        public bool GetProcess()
		{
            // get the process
            Process process = WinApi.GetProcess(_processName);
            if (process == null || process.HasExited) {
                _process = null;
                return false;
            }
            _process = process;

            if (_useVolume) {
                // find the required addresses
                int moduleAddress = WinApi.GetModuleAddress(process, _volumeModuleName);
                // todo: There's overlap in this code with Game.cs , maybe make them inherit this from somewhere
                if (moduleAddress == -1) {
                    // TODO: requested module not found. This can happen while the process boots, or if it's not configured properly. Display a Message
                    _process = null;
                    return false;
                }
                _addressVolume = moduleAddress + _volumeAddressOffset;
                _volume = ReadVolume();
            }

            if (_hookMethod == HookMethods.SendMessage) {
                _window = WinApi.FindWindow(_windowName, null);
            }
            else if (_hookMethod == HookMethods.ProcessStart) {
                _executableFile = _process.MainModule.FileName;
                _processStartInfo = new ProcessStartInfo();
                _processStartInfo.FileName = Path.GetFileName(_executableFile);
                _processStartInfo.WorkingDirectory = Path.GetDirectoryName(_executableFile);
                _processStartInfo.Arguments = _arguments;
            }

            return true;
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
        /// Set radio to on or off
        /// </summary>
        /// <param name="mute">whether to turn the radio on</param>
        public void Mute(bool mute = true) {
            if (!IsRunning()) {
                //throw new WarningException("No music player is running.");
            }
            if (!_useVolume) {
                ToggleMute();
                return;
            }

            switch (_hookMethod) {
                case HookMethods.SendMessage:
                    // TODO: take care of all the other hookmethods for these methods
                    int wParam;
                    if (!mute) {   
                        // turn radio on
                        wParam = _volume;
                    }
                    else {      
                        // turn radio off
                        _volume = ReadVolume();
                        wParam = _minVolume;
                    }
                    WinApi.SendMessage(_window, _wMsg, wParam, _lParam);
                    break;
                case HookMethods.ProcessStart:
                    throw new NotImplementedException("ProcessStart hookmethod hasn't been implemented for volume control yet"); //todo:
                    break;
            }
        }

        public void ToggleMute() {
            if (!IsRunning()) {
                //
            }
            int wParam = 0;     // todo: configure wParam
            switch (_hookMethod) {
                case HookMethods.SendMessage:
                    throw new NotImplementedException("SendMessage hookmethod can only be used if combination with volume reading so far"); //todo:
                    break;
                case HookMethods.ProcessStart:
                    Process.Start(_processStartInfo);
                    break;
            }
        }

        int ReadVolume() {
            if (!IsRunning()) {
                //throw new WarningException("No music player is running.");
            }

            int volume = -1;
            try {
                volume = WinApi.ReadValue(_process, _addressVolume, _volumeValueType);
            }
            catch {
                // TODO: catch errors
            }
            Debug.WriteLine(volume);
            return volume;
        }

	}
}
