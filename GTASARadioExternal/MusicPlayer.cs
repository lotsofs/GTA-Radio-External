using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


namespace GTASARadioExternal
{
    public class MusicPlayer
	{
        public enum Statuses {
            Uninitialized,
            Shutdown,
            Stopped,
            Playing
        }

        public enum HookMethods {
            None,
            SendMessage,    // for winamp, spotify
            ProcessStart,    // for foobar
            MemoryWrite     // for anything else?
        }

        Process _process;
        int _musicPlayerWindow = 0;
        int _volume = 0;
		int _addressVolume = 0x0;
        int _addressRunning = 0x0;
        Statuses _status;
        HookMethods _hookMethod;

        public MusicPlayer() {
            Configure();
            GetProcess();
        }

        public void Configure() {
            // TODO: move settings file stuff here
        }

        /// <summary>
        /// Reinitializes the music player: Finds a process for the music player and configures it if found. Returns whether it found something or not.
        /// </summary>
        /// <returns></returns>
        public bool GetProcess()
		{
            _status = Statuses.Uninitialized;
            _hookMethod = HookMethods.None;

            Process process = WinApi.GetProcess(Settings.ProcessName);
            if (process == null) {
                _status = Statuses.Shutdown;
                _process = null;
                return false;
            }
            //if (IsRunning() == false) {
            //    _status = Statuses.Stopped;
            //}
            //else {
            //    _status = Statuses.Playing;
            //}
            _status = Statuses.Playing;
             _process = process;
            
            int moduleAddress = WinApi.GetModuleAddress(process, Settings.VolumeModuleName);
            _addressVolume = moduleAddress + Settings.VolumeAddressOffset;

            //moduleAddress = WinApi.GetModuleAddress(process, Settings.RunningModuleName);
            //_addressRunning = moduleAddress + Settings.RunningAddressOffset;

            if (!string.IsNullOrEmpty(Settings.WindowName)) {   // todo: add an actual setting where the user can just select this
                _hookMethod = HookMethods.SendMessage;
                _musicPlayerWindow = WinApi.FindWindow(Settings.WindowName, null);
            }
            else if (!string.IsNullOrEmpty(Settings.ProcessArguments)) {
                _hookMethod = HookMethods.ProcessStart;
                // TODO: Implement the below snippet from old code that would mute Foobar into this new code
                //ProcessStartInfo psi = new ProcessStartInfo();
                //psi.FileName = Path.GetFileName(executable_location);
                //psi.WorkingDirectory = Path.GetDirectoryName(executable_location);
                //psi.Arguments = "/command:mute";
                //Process.Start(psi);
            }
            else {
                _hookMethod = HookMethods.None;
            }
            return true;
        }

        /// <summary>
        /// Set radio to on or off
        /// </summary>
        /// <param name="mute">whether to turn the radio on</param>
        public void Mute(bool mute) {
            if (_status == Statuses.Shutdown) {
                throw new WarningException("No music player is running.");
            }

            // TODO: take care of all the other hookmethods for these methods
            if (!mute) {   // turn radio on
                WinApi.SendMessage(_musicPlayerWindow, 0x0400, _volume, Settings.MessageLParam); //0x0400 = WM_USER
            }
            else {      // turn radio off
                _volume = ReadVolume();
                WinApi.SendMessage(_musicPlayerWindow, 0x0400, 0, Settings.MessageLParam); //0x0400 = WM_USER
            }
        }

        /// <summary>
        /// toggles the radio on or off
        /// </summary>
        public void ToggleMute() {
            if (_status == Statuses.Shutdown) {
                throw new WarningException("No music player is running.");
            }

            int volume = ReadVolume();
            if (volume == 0) {  // turn radio on
                WinApi.SendMessage(_musicPlayerWindow, 0x0400, _volume, Settings.MessageLParam); //0x0400 = WM_USER
            }
            else {              // turn radio off
                _volume = volume;
                WinApi.SendMessage(_musicPlayerWindow, 0x0400, 0, Settings.MessageLParam); //0x0400 = WM_USER
            }
        }

        [Obsolete("Only exists to avoid a rare infinite recursion bug. Please deal with it where it happens")]
        bool IsRunning() {
            if (_status == Statuses.Shutdown) {
                throw new WarningException("No music player is running.");
            }

            int running = 0;
            try {
                running = WinApi.ReadValue(_process, _addressRunning, Settings.RunningAddressType);
            }
            catch {
                // TODO: catch errors
            }
            return running != 0;
        }

        int ReadVolume() {
            if (_status == Statuses.Shutdown) {
                throw new WarningException("No music player is running.");
            }

            int volume = -1;
            try {
                volume = WinApi.ReadValue(_process, _addressVolume, Settings.VolumeAddressType);
            }
            catch {
                // TODO: catch errors
            }
            return volume;
        }

	}
}
