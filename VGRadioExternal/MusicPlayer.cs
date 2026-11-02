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
        long _addressVolume;
        int _volume;
        
        ProcessStartInfo _processStartInfo;

        int _window;
        string _executableFile;
        bool _toggledMuted;

        public string Status { get; private set; }

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
            try {
                // get the process
                Process process = WinApi.GetProcess(_mp.ProcessName);
                if (process == null || process.HasExited) {
                    _process = null;
                    _toggledMuted = false;
                    Status = "Not running";
                    return false;
                }
                _process = process;

                if (_mp.ReadVolume != null) {
                    // find the required addresses
                    long moduleAddress = WinApi.GetModuleAddress(process, _mp.ReadVolume.Module);
                    if (moduleAddress == -1) {
                        // requested module not found. This can happen while the process boots, or if it's not configured properly.
                        _process = null;
                        Status = "Missing " + _mp.ReadVolume.Module;
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


        /// <summary>
        /// Set radio to on or off
        /// </summary>
        /// <param name="mute">whether to turn the radio on</param>
        public void Mute(bool mute = true) {
            if (!IsRunning()) {
                //throw new WarningException("No music player is running.");
            }
            if (_mp.SetVolume.Type == HookMethods.AudioSession) {
                // an app only has a session once it has played something, so there may be nothing to unmute
                if (AudioSessions.SetMute(_mp.ProcessName, mute) == 0 && mute) {
                    // Radio tries again every poll, so a session that appears later still gets muted
                    throw new InvalidOperationException("No audio session yet. Play something in " + _mp.ProcessName + ".");
                }
                return;
            }
            if (_mp.ReadVolume == null || _mp.SetVolume.Type == HookMethods.KeyPress) {
                ToggleMute(mute);
                return;
            }

            switch (_mp.SetVolume.Type) {
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
                    else { wParam = int.Parse(_mp.SetVolume.WParam); }
                    if (_mp.SetVolume.LParam == "{volume}") { lParam = destinationVolume; }
                    else { lParam = int.Parse(_mp.SetVolume.LParam); }
                    
                    int wMsg = (int)Enum.Parse(typeof(WinApi.wMsg), _mp.SetVolume.WMsg);
                    WinApi.SendMessage(_window, wMsg, wParam, lParam);
                    break;
                case HookMethods.ProcessStart:
                    throw new NotImplementedException("ProcessStart hookmethod hasn't been implemented for volume control yet");
                    break;
            }
        }

        /// <summary>
        /// Mutes or unmutes a player that can only be toggled. Nothing says which state it's in, so this assumes it starts unmuted and that only this tool toggles it.
        /// </summary>
        /// <param name="mute">whether the player should end up muted</param>
        public void ToggleMute(bool mute) {
            if (!IsRunning()) {
                //
            }
            if (mute == _toggledMuted) {
                return;
            }
            int wParam = 0;
            switch (_mp.SetVolume.Type) {
                case HookMethods.SendMessage:
                    throw new NotImplementedException("SendMessage hookmethod can only be used if combination with volume reading so far");
                    break;
                case HookMethods.ProcessStart:
                    Process.Start(_processStartInfo);
                    break;
                case HookMethods.KeyPress:
                    WinApi.PressKeys(_mp.SetVolume.Keys);
                    break;
            }
            _toggledMuted = mute;
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
                // a failed read leaves volume at int.MinValue
            }
            Debug.WriteLine(volume);
            return volume;
        }

	}
}
