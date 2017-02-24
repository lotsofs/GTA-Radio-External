using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace GTASARadioExternal {
	public class ReadMemory {

		public int major = 0;
		public int minor = 0;
		public enum regionTypes { US, Europe, Unknown };
		public regionTypes region = regionTypes.Unknown;
		public enum statuses { Unitialized, Shutdown, Running, Unrecognized, Unconfirmed, Playing, Silent }
		public statuses gameStatus = statuses.Unitialized;
		public statuses playerStatus = statuses.Unitialized;
		public Process[] p;
		public Process[] q;
		public int address_radio = 0x0;     // The address of the int that changes depending on radio status
		public int address_volume = 0x0;    // The address of the int that reads the volume of the music player
		public int address_running = 0x0;   // The address of the int that reads whether the music player is on or not
		Timer timer1;
		public bool maxVolumeWriteable = true;
		public bool quickVolume = false;
		public bool isPaused = false;
		public enum actions { None, Volume, Mute, Pause }
		public actions actionToTake;

		public int volumeStatus;
		public int radioStatus;
		public int prevRadioStatus;
		public int maxVolume;

		// Dont know what this does
		const int PROCESS_WM_READ = 0x0010;

		// Allows me to read memory from processes
		[DllImport("kernel32.dll")]
		public static extern Int32 ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress,
		  [In, Out] byte[] buffer, UInt32 size, out IntPtr lpNumberOfBytesRead);

		// Send keystrokes for volume
		[DllImport("user32.dll")]
		public static extern void keybd_event(byte virtualKey, byte scanCode, uint flags, IntPtr extraInfo);

		/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * 
					* Game Detection
		* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

		// Determine the version of GTASA
		public void DetermineGameVersionSA() {
			p = Process.GetProcessesByName("gta-sa");
			if (p.Length == 0) {
				p = Process.GetProcessesByName("gta_sa");
			}
			if (p.Length != 0) {
				if (ReadInt32(p[0].Handle, 0x82457C) == 38079) {        // 1.0 US
					major = 1; minor = 0; region = regionTypes.US; gameStatus = statuses.Running;
					address_radio = 0x008CB760;
				}
				else if (ReadInt32(p[0].Handle, 0x8245BC) == 38079) {   // 1.0 EU
					major = 1; minor = 0; region = regionTypes.Europe; gameStatus = statuses.Running;
					address_radio = 0x008CB760;
				}
				else if (ReadInt32(p[0].Handle, 0x8252FC) == 38079) {       // 1.1 US
					major = 1; minor = 1; region = regionTypes.US; gameStatus = statuses.Running;
					address_radio = 0x008CCFE8; gameStatus = statuses.Unconfirmed;
				}
				else if (ReadInt32(p[0].Handle, 0x82533C) == 38079) {       // 1.1 EU
					major = 1; minor = 1; region = regionTypes.Europe; gameStatus = statuses.Running;
					address_radio = 0x008CCFE8; gameStatus = statuses.Unconfirmed;
				}
				else if (ReadInt32(p[0].Handle, 0x85EC4A) == 38079) {       // 3.0 Steam
					major = 3; minor = 0; region = regionTypes.Unknown; gameStatus = statuses.Running;
					address_radio = 0x0093AB68;
				}
				else {
					gameStatus = statuses.Unrecognized;
				}
			}
			else {
				gameStatus = statuses.Shutdown;
			}
		}

		/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * 
						* Music Player Detection
		* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

		// Determine the version of Winamp
		public void DeterminePlayerVersionWinamp() {
			q = Process.GetProcessesByName("winamp");
			if (q.Length != 0) {
				playerStatus = statuses.Running;
				address_volume = 0x07AA9D9C;
				address_running = 0x4BF3EC;
			}
			else {
				playerStatus = statuses.Shutdown;
			}
		}

		// This isn't actually going to detect anything
		public void DeterminePlayerVersionOther() {
			playerStatus = statuses.Running;
		}


		/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * 
					* Radio Status Detection
		* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */


		public void CheckRadioStatus() {
			if (playerStatus != statuses.Running) {
				return;
			}

			if (actionToTake == actions.Pause) {
				if (gameStatus == statuses.Running) {
					try {
						radioStatus = ReadInt32(p[0].Handle, address_radio);
					}
					catch (InvalidOperationException) {
						Debug.WriteLine("It should write this line, but for some reason it doesn't. Ah well, at least the unhandled exception doesn't show up anymore");
						return;
					}
					if (radioStatus == 2 && isPaused == true) {
						isPaused = false;
						RadioChangerPause();
					}
					else if (radioStatus == 7 && isPaused == false) {
						isPaused = true;
						RadioChangerPause();
					}
				}
			}
			else if (actionToTake == actions.Volume) {
				// Unless the radio is currently changing, allow user to change volume
				if (maxVolumeWriteable == true && radioStatus == 2 || gameStatus != statuses.Running) {
					maxVolume = checkMP3PlayerStatus();
				}
				prevRadioStatus = radioStatus;

				if (gameStatus == statuses.Running) {
					try {
						radioStatus = ReadInt32(p[0].Handle, address_radio);
					}
					catch (InvalidOperationException) {
						Debug.WriteLine("It should write this line, but for some reason it doesn't. Ah well, at least the unhandled exception doesn't show up anymore");
						return;
					}


					volumeStatus = checkMP3PlayerStatus();
					RadioChangerVolume();
				}
			}
		}

		// Change Radio based on pausing/unpausing
		void RadioChangerPause() {
			keybd_event(0xB3, 0, 1, IntPtr.Zero);
			keybd_event(0xB3, 0, 2, IntPtr.Zero);
		}

		// Change Radio based on Volume slider
		void RadioChangerVolume() {
			if (radioStatus == 2 && volumeStatus < maxVolume) {
				// radio should be on but volume is too low
				maxVolumeWriteable = false;
				keybd_event(0xAF, 0, 1, IntPtr.Zero);
				if (quickVolume) {
					keybd_event(0xAF, 0, 1, IntPtr.Zero);
					keybd_event(0xAF, 0, 1, IntPtr.Zero);
				}
				keybd_event(0xAF, 0, 2, IntPtr.Zero);
				volumeStatus = checkMP3PlayerStatus();
			}
			else if (radioStatus == 7 && volumeStatus > 0) {
				// radio should be off but volume isn't 0
				while (volumeStatus > 0) {
					keybd_event(0XAE, 0, 1, IntPtr.Zero);
					keybd_event(0XAE, 0, 2, IntPtr.Zero);
					volumeStatus = checkMP3PlayerStatus();
				}
			}
			else if (maxVolumeWriteable == false) {
				maxVolumeWriteable = true;
			}
		}


		// volume slider is used for checking if radio is on or off (winamp didn't want to let me take control of its mute button)
		private int checkMP3PlayerStatus() {
			int volumeLevel = -1;
			if (playerStatus == statuses.Running) {
				try {
					volumeLevel = ReadInt32(q[0].Handle, address_volume);
				}
				catch (InvalidOperationException) {
					Debug.WriteLine("It should write this line, but for some reason it doesn't. Ah well, at least the unhandled exception doesn't show up anymore");
					return -1;
				}

				// If it returns 255, make sure it isn't glitching, which winamp likes to do if it hasn't been turned on yet
				if (volumeLevel == 255) {
					int activity;
					activity = checkMP3ActiveStatus();
					return volumeLevel * activity;
				}
				return volumeLevel;
			}
			else {
				return -1;
			}
		}

		// This function checks whether winamp is actually playing. To prevent crash if winamp has just been booted but hasn't started music yet.
		private int checkMP3ActiveStatus() {
			int playerActive = 0;
			if (playerStatus == statuses.Running) {
				try {
					playerActive = ReadInt32(q[0].Handle, address_running);
				}
				catch (InvalidOperationException) {
					Debug.WriteLine("It should write this line, but for some reason it doesn't. Ah well, at least the unhandled exception doesn't show up anymore");
					return 0;
				}
				return playerActive;
			}
			else {
				return 0;
			}
		}

		// timer that updates radio status every frame or so
		public void InitTimer() {
			timer1 = new Timer();
			timer1.Tick += new EventHandler(Timer1Tick);
			timer1.Interval = 40;
			timer1.Start();
		}

		void Timer1Tick(object sender, EventArgs e) {
			CheckRadioStatus();
		}


		// Bitconverter to return whatever is in that memory address to an integer so I can work with it
		private static int ReadInt32(IntPtr handle, long address) {
			return BitConverter.ToInt32(ReadBytes(handle, address, 4), 0);
		}

		// Read memory
		private static byte[] ReadBytes(IntPtr handle, long address, uint bytesToRead) {
			IntPtr ptrBytesRead;
			byte[] buffer = new byte[bytesToRead];
			ReadProcessMemory(handle, new IntPtr(address), buffer, bytesToRead, out ptrBytesRead);
			return buffer;
		}


	}
}
