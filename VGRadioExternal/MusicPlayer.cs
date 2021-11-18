//using S.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace VGRadioExternal
{
    public class MusicPlayer
	{

        MPData _mp;

        Process _process;
		int _addressVolume;
        int _volume;
        
        ProcessStartInfo _processStartInfo;

        int _window;
        string _executableFile;

        public MusicPlayer(string json) {
            _mp = JsonConvert.DeserializeObject<MPData>(json);
            GetProcess();
            //Mute();
        }

        /// <summary>
        /// Reinitializes the music player: Finds a process for the music player and configures it if found. Returns whether it found something or not.
        /// </summary>
        /// <returns></returns>
        public bool GetProcess()
		{
            // get the process
            Process process = WinApi.GetProcess(_mp.ProcessName);
            if (process == null || process.HasExited) {
                _process = null;
                return false;
            }
            _process = process;

            if (_mp.ReadVolume != null) {
                // find the required addresses
                int moduleAddress = WinApi.GetModuleAddress(process, _mp.ReadVolume.Module);
                // todo: There's overlap in this code with Game.cs , maybe make them inherit this from somewhere
                if (moduleAddress == -1) {
                    // TODO: requested module not found. This can happen while the process boots, or if it's not configured properly. Display a Message
                    _process = null;
                    return false;
                }
                _addressVolume = moduleAddress + _mp.ReadVolume.AddressOffset;
                _volume = ReadVolume();
            }

            if (_mp.SetVolume.Type == HookMethods.SendMessage) {
                _window = WinApi.FindWindow(_mp.SetVolume.WindowName, null);
            }
            else if (_mp.SetVolume.Type == HookMethods.ProcessStart) {
                _executableFile = _process.MainModule.FileName;
				_processStartInfo = new ProcessStartInfo {
					FileName = Path.GetFileName(_executableFile),
					WorkingDirectory = Path.GetDirectoryName(_executableFile),
					Arguments = _mp.SetVolume.Arguments
				};
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
            if (_mp.ReadVolume == null) {
                ToggleMute();
                return;
            }

            switch (_mp.SetVolume.Type) {
                // TODO: take care of all the other hookmethods for these methods
                case HookMethods.SendMessage:
                    int destinationVolume;
                    if (!mute) {
                        // turn radio on
                        destinationVolume = _volume;
                    }
                    else {
                        // turn radio off, store old volume
                        _volume = ReadVolume();
                        destinationVolume = _mp.MinVolume;
					}

                    int wParam;
                    int lParam;
                    if (_mp.SetVolume.WParam == "{volume}") { wParam = destinationVolume; }
                    else { wParam = int.Parse(_mp.SetVolume.WParam); } // todo: Validate? Or is that done in the json reader?
                    if (_mp.SetVolume.LParam == "{volume}") { lParam = destinationVolume; }
                    else { lParam = int.Parse(_mp.SetVolume.LParam); }
                    
                    int wMsg = (int)Enum.Parse(typeof(WinApi.wMsg), _mp.SetVolume.WMsg);
                    WinApi.SendMessage(_window, wMsg, wParam, lParam);
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
            switch (_mp.SetVolume.Type) {
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

            int volume = int.MinValue;
            try {
                WinApi.Types volumeValueType = (WinApi.Types)Enum.Parse(typeof(WinApi.Types), _mp.ReadVolume.Type);
                volume = WinApi.ReadValue(_process, _addressVolume, volumeValueType);
            }
            catch {
                // TODO: catch errors
            }
            Debug.WriteLine(volume);
            return volume;
        }

	}
}
