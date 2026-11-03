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
        // how long a new process gets to set up its window, hotkey and volume before it's told anything
        static readonly TimeSpan StartupDelay = TimeSpan.FromSeconds(3);

        MPData _mp;

        Process _process;
        long _addressVolume;
        // the volume to go back to, while this tool has the player at minVolume. It outlives the process, in case the player saves minVolume on exit
        int? _volume;
        
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
            // this only runs once any process it toggled has exited, and a new one starts unmuted
            _toggledMuted = false;
            try {
                // get the process
                Process process = WinApi.GetProcess(_mp.ProcessName);
                if (process == null || process.HasExited) {
                    _process = null;
                    Status = "Not running";
                    return false;
                }
                if (DateTime.Now - process.StartTime < StartupDelay) {
                    // anything sent before the player is ready is lost, and a lost toggle would leave it inverted
                    _process = null;
                    Status = "Starting";
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
            if (_mp.ReadVolume == null) {
                ToggleMute(mute);
                return;
            }
            if (_mp.SetVolume.Type == HookMethods.SendMessage && CarriesVolume()) {
                if (mute) {
                    // turn radio off, store old volume. A second mute keeps it, because the player is at minVolume by then
                    _volume = _volume ?? ReadVolume();
                    SendConfiguredMessage(_mp.MinVolume);
                }
                else if (_volume != null) {
                    // turn radio on. Without a stored volume this tool hasn't lowered it, so there's nothing to undo
                    SendConfiguredMessage(_volume.Value);
                    _volume = null;
                }
                return;
            }
            // the volume shows which state the player is in, so unlike a blind toggle this can't end up inverted
            if ((ReadVolume() <= _mp.MinVolume) != mute) {
                Toggle();
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
            Toggle();
            _toggledMuted = mute;
        }

        /// <summary>
        /// Flips the player's mute through the hook the config names
        /// </summary>
        void Toggle() {
            switch (_mp.SetVolume.Type) {
                case HookMethods.SendMessage:
                    SendConfiguredMessage(_mp.MinVolume);
                    break;
                case HookMethods.ProcessStart:
                    Process.Start(_processStartInfo);
                    break;
                case HookMethods.KeyPress:
                    WinApi.PressKeys(_mp.SetVolume.Keys);
                    break;
            }
        }

        bool CarriesVolume() {
            return _mp.SetVolume.WParam == "{volume}" || _mp.SetVolume.LParam == "{volume}";
        }

        /// <summary>
        /// Sends the window message from the config
        /// </summary>
        /// <param name="volume">what {volume} stands for in wParam and lParam</param>
        void SendConfiguredMessage(int volume) {
            int wParam = MessageParameter(_mp.SetVolume.WParam, volume);
            int lParam = MessageParameter(_mp.SetVolume.LParam, volume);
            int wMsg = (int)Enum.Parse(typeof(WinApi.wMsg), _mp.SetVolume.WMsg);
            WinApi.SendMessage(_window, wMsg, wParam, lParam);
        }

        int MessageParameter(string parameter, int volume) {
            if (parameter == "{volume}") { return volume; }
            return int.Parse(parameter);
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
