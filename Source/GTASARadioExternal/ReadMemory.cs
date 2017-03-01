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
		public enum regionTypes { GTA, US, Europe, Japan, Steam };
		public regionTypes region = regionTypes.GTA;
		public enum statuses { Unitialized, Shutdown, Running, Unrecognized, Unconfirmed, Playing, Silent, Error }
		public statuses gameStatus = statuses.Unitialized;
		public enum games { None, III, VC, SA }
		public games game = games.None;
		public enum musicPlayers { None, Winamp, Foobar, Other }
		public musicPlayers musicP = musicPlayers.None;
		public statuses playerStatus = statuses.Unitialized;
		public Process[] p;
		public Process[] q;
		public int address_radio = 0x0;     // The address of the int that changes depending on radio status
		public int address_volume = 0x0;    // The address of the int that reads the volume of the music player
		public int address_running = 0x0;   // The address of the int that reads whether the music player is on or not
		public int address_base = 0x0;
		Timer timer1;
		public bool maxVolumeWriteable = true;
		public bool quickVolume = false;
		public bool isPaused = false;
		public enum actions { None, Volume, Mute, Pause }
		public actions actionToTake;

		public int failSafeAttempts = 0;

		public int prevVolumeStatus;
		public int volumeStatus;
		public int radioStatus;
		public int prevRadioStatus;
		public int maxVolume;
		public bool radioActive = false;
		public bool ignoreMods = false;

		//public int radioLowerBoundary = 0;
		//public int radioUpperBoundary = 10;
		public bool radioPlayDuringPauseMenu;
		public bool radioPlayDuringRadio;
		public bool radioPlayDuringEmergency;
		public bool radioPlayDuringKaufman;
		public bool radioPlayDuringAnnouncement;
		public bool radioPlayDuringInterior;

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

		// Determine the version of GTA3
		public void DetermineGameVersionIII() {
			p = Process.GetProcessesByName("gta3");
			if (p.Length != 0) {
				if (ReadValue(p[0].Handle, 0x5C1E70, false, true) == 1407551829) {        // 1.0
					major = 1; minor = 0; region = regionTypes.GTA; gameStatus = statuses.Running;
					address_radio = 0x8F3967;
				}
				else if (ReadValue(p[0].Handle, 0x5C2130, false, true) == 1407551829) {   // 1.1
					major = 1; minor = 1; region = regionTypes.GTA; gameStatus = statuses.Running;
					address_radio = 0x8F3A1B; 
				}
				else if (ReadValue(p[0].Handle, 0x5C6FD0, false, true) == 1407551829) {       // 1.1 Steam
					major = 1; minor = 1; region = regionTypes.Steam; gameStatus = statuses.Running;
					address_radio = 0x903B5C; ;
				}
				else {
					gameStatus = statuses.Unrecognized;
				}
			}
			else {
				gameStatus = statuses.Shutdown;
			}
		}

		// Determine the version of GTAVC
		public void DetermineGameVersionVC() {
			p = Process.GetProcessesByName("gta-vc");
			if (p.Length != 0) {
				if (ReadValue(p[0].Handle, 0x667BF0, false, true) == 1407551829) {        // 1.0
					major = 1; minor = 0; region = regionTypes.GTA; gameStatus = statuses.Running;
					address_radio = 0x9839C0;
				}
				else if (ReadValue(p[0].Handle, 0x667C40, false, true) == 1407551829) {   // 1.1
					major = 1; minor = 1; region = regionTypes.GTA; gameStatus = statuses.Running;
					address_radio = 0x9839C0; gameStatus = statuses.Unconfirmed;
				}
				else if (ReadValue(p[0].Handle, 0xA402ED, false, true) == 1448235347) {       // 1.1 Steam
					major = 1; minor = 1; region = regionTypes.Steam; gameStatus = statuses.Running;
					address_radio = 0x9829C8;
				}
				else if (ReadValue(p[0].Handle, 0xACD0A2, false, true) == 1793887061) {       // 1.1 JP
					major = 1; minor = 1; region = regionTypes.Japan; gameStatus = statuses.Running;
					address_radio = 0x9809D0;
				}
				else {
					gameStatus = statuses.Unrecognized;
				}
			}
			else {
				gameStatus = statuses.Shutdown;
			}
		}
		
		// Determine the version of GTASA
		public void DetermineGameVersionSA() {
			p = Process.GetProcessesByName("gta-sa");
			if (p.Length == 0) {
				p = Process.GetProcessesByName("gta_sa");
			}
			if (p.Length != 0) {
				if (ReadValue(p[0].Handle, 0x82457C, false, true) == 38079) {        // 1.0 US
					major = 1; minor = 0; region = regionTypes.US; gameStatus = statuses.Running;
					address_radio = 0x008CB760;
				}
				else if (ReadValue(p[0].Handle, 0x8245BC, false, true) == 38079) {   // 1.0 EU
					major = 1; minor = 0; region = regionTypes.Europe; gameStatus = statuses.Running;
					address_radio = 0x008CB760;
				}
				else if (ReadValue(p[0].Handle, 0x8252FC, false, true) == 38079) {       // 1.1 US
					major = 1; minor = 1; region = regionTypes.US; gameStatus = statuses.Running;
					address_radio = 0x008CCFE8; gameStatus = statuses.Unconfirmed;
				}
				else if (ReadValue(p[0].Handle, 0x82533C, false, true) == 38079) {       // 1.1 EU
					major = 1; minor = 1; region = regionTypes.Europe; gameStatus = statuses.Running;
					address_radio = 0x008CCFE8; gameStatus = statuses.Unconfirmed;
				}
				else if (ReadValue(p[0].Handle, 0x85EC4A, false, true) == 38079) {       // 3.0 Steam
					major = 3; minor = 0; region = regionTypes.Steam; gameStatus = statuses.Running;
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

		// Determine the version of Foobar
		public void DeterminePlayerVersionFoobar() {
			q = Process.GetProcessesByName("foobar2000");
			if (q.Length != 0) {
				playerStatus = statuses.Running;
				address_base = q[0].MainModule.BaseAddress.ToInt32();
				address_volume = address_base + 0x18C438;
				address_running = address_base + 0x18B1F0;
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

		public void CheckRadioStatusIII() {
			if (playerStatus != statuses.Running) {
				return;
			}

			if (actionToTake == actions.Pause) {
				if (gameStatus == statuses.Running || gameStatus == statuses.Unconfirmed) {
					#region try catch radiostatus
					try {
						radioStatus = ReadValue(p[0].Handle, address_radio, false, false);
					}
					catch (InvalidOperationException) {
						Debug.WriteLine("InvalidOperationException A");
						gameStatus = statuses.Shutdown;
						return;
					}
					catch (NullReferenceException) {
						Debug.WriteLine("NullReferenceException A");
						gameStatus = statuses.Shutdown;
						return;
					}
					catch (IndexOutOfRangeException) {
						Debug.WriteLine("IndexOutOfRangeException A");
						gameStatus = statuses.Shutdown;
						return;
					}
					#endregion

					#region pause and unpause condition clauses
					if (radioStatus >= 0 && radioStatus <= 9 && radioPlayDuringRadio == true && isPaused == true) {
						isPaused = false;
						RadioChangerPause(isPaused);
					}
					else if (radioStatus == 10 && radioPlayDuringEmergency == true && isPaused == true) {
						isPaused = false;
						RadioChangerPause(isPaused);
					}
					else if (radioStatus == 197 && radioPlayDuringPauseMenu == true && isPaused == true) {
						isPaused = false;
						RadioChangerPause(isPaused);
					}
					if (radioStatus >= 13 && radioStatus <= 14 && radioPlayDuringAnnouncement == true && isPaused == true) {
						isPaused = false;
						RadioChangerPause(isPaused);
					}


					else if (radioStatus == 197 && radioPlayDuringPauseMenu == false && isPaused == false) {
						isPaused = true;
						RadioChangerPause(isPaused);
					}
					else if (radioStatus == 10 && radioPlayDuringEmergency == false && isPaused == false) {
						isPaused = true;
						RadioChangerPause(isPaused);
					}
					else if (radioStatus >= 0 && radioStatus <= 9 && radioPlayDuringRadio == false && isPaused == false) {
						isPaused = true;
						RadioChangerPause(isPaused);
					}
					else if (radioStatus >= 13 && radioStatus <= 14 && radioPlayDuringAnnouncement == false && isPaused == false) {
						isPaused = true;
						RadioChangerPause(isPaused);
					}

					else if (radioStatus > 10 && radioStatus != 197 && radioStatus != 13 && radioStatus != 14 && isPaused == false) {
						isPaused = true;
						RadioChangerPause(isPaused);
					}
					#endregion
				}
			}
			else if (actionToTake == actions.Volume) {
				// Unless the radio is currently changing, allow user to change volume
				#region maxvolume changer
				if (gameStatus != statuses.Running && gameStatus != statuses.Unconfirmed) {
					maxVolume = checkMP3PlayerStatus();
				}
				else if (maxVolumeWriteable && radioStatus >= 0 && radioStatus <= 9 && radioPlayDuringRadio == true) {
					maxVolume = checkMP3PlayerStatus();
				}
				else if (maxVolumeWriteable && radioStatus == 10 && radioPlayDuringEmergency == true) {
					maxVolume = checkMP3PlayerStatus();
				}
				else if (maxVolumeWriteable && radioStatus == 197 && radioPlayDuringPauseMenu == true) {
					maxVolume = checkMP3PlayerStatus();
				}
				#endregion
				prevRadioStatus = radioStatus;

				if (gameStatus == statuses.Running || gameStatus == statuses.Unconfirmed) {
					#region try catch radiostatus
					try {
						radioStatus = ReadValue(p[0].Handle, address_radio, false, false);
					}
					catch (InvalidOperationException) {
						Debug.WriteLine("InvalidOperationException B");
						gameStatus = statuses.Shutdown;
						return;
					}
					catch (NullReferenceException) {
						Debug.WriteLine("NullReferenceException B");
						gameStatus = statuses.Shutdown;
						return;
					}
					catch (IndexOutOfRangeException) {
						Debug.WriteLine("IndexOutOfRangeException B");
						gameStatus = statuses.Shutdown;
						return;
					}
					#endregion

					RadioChangerVolume(radioStatus >= 0 && radioStatus <= 9 && radioPlayDuringRadio 
											|| radioStatus == 10 && radioPlayDuringEmergency == true 
											|| radioStatus >= 13 && radioStatus <= 14 && radioPlayDuringAnnouncement == true
											|| radioStatus == 197 && radioPlayDuringPauseMenu == true
					);
				}
			}
		}

		public void CheckRadioStatusVC() {
			if (playerStatus != statuses.Running) {
				return;
			}

			if (actionToTake == actions.Pause) {
				if (gameStatus == statuses.Running || gameStatus == statuses.Unconfirmed) {
					try {
						radioStatus = ReadValue(p[0].Handle, address_radio, false, true);
					}
					catch (InvalidOperationException) {
						Debug.WriteLine("InvalidOperationException C");
						gameStatus = statuses.Shutdown;
						return;
					}
					catch (NullReferenceException) {
						Debug.WriteLine("NullReferenceException C");
						gameStatus = statuses.Shutdown;
						return;
					}
					catch (IndexOutOfRangeException) {
						Debug.WriteLine("IndexOutOfRangeException C");
						gameStatus = statuses.Shutdown;
						return;
					}

					if (radioStatus >= 0 && radioStatus <= 9  && radioPlayDuringRadio == true && isPaused == true) {
						isPaused = false;
						RadioChangerPause(isPaused);
					}
					if (radioStatus >= 25 && radioStatus <= 26 && radioPlayDuringAnnouncement == true && isPaused == true) {
						isPaused = false;
						RadioChangerPause(isPaused);
					}
					else if (radioStatus == 23 && radioPlayDuringEmergency == true && isPaused == true) {
						isPaused = false;
						RadioChangerPause(isPaused);
					}
					else if (radioStatus == 1225 && radioPlayDuringPauseMenu == true && isPaused == true) {
						isPaused = false;
						RadioChangerPause(isPaused);
					}
					else if (radioStatus == 24 && radioPlayDuringKaufman == true && isPaused == true) {
						isPaused = false;
						RadioChangerPause(isPaused);
					}

					else if (radioStatus == 1225 && radioPlayDuringPauseMenu == false && isPaused == false) {
						isPaused = true;
						RadioChangerPause(isPaused);
					}
					else if (radioStatus >= 25 && radioStatus <= 26 && radioPlayDuringAnnouncement == false && isPaused == false) {
						isPaused = true;
						RadioChangerPause(isPaused);
					}
					else if (radioStatus == 23 && radioPlayDuringEmergency == false && isPaused == false) {
						isPaused = true;
						RadioChangerPause(isPaused);
					}
					else if (radioStatus == 24 && radioPlayDuringKaufman == false && isPaused == false) {
						isPaused = true;
						RadioChangerPause(isPaused);
					}
					else if (radioStatus >= 0 && radioStatus <= 9 && radioPlayDuringRadio == false && isPaused == false) {
						isPaused = true;
						RadioChangerPause(isPaused);
					}

					else if (radioStatus > 9 && radioStatus < 23 && isPaused == false || radioStatus > 26 && radioStatus != 1225 && isPaused == false) {
						isPaused = true;
						RadioChangerPause(isPaused);
					}
				}
			}
			else if (actionToTake == actions.Volume) {
				// Unless the radio is currently changing, allow user to change volume
				if (gameStatus != statuses.Running && gameStatus != statuses.Unconfirmed) {
					maxVolume = checkMP3PlayerStatus();
				}
				else if (maxVolumeWriteable && radioStatus >= 0 && radioStatus <= 9 && radioPlayDuringRadio == true) {
					maxVolume = checkMP3PlayerStatus();
				}
				else if (maxVolumeWriteable && radioStatus >= 25 && radioStatus <= 26 && radioPlayDuringAnnouncement == true) {
					maxVolume = checkMP3PlayerStatus();
				}
				else if (maxVolumeWriteable && radioStatus == 23 && radioPlayDuringEmergency == true) {
					maxVolume = checkMP3PlayerStatus();
				}
				else if (maxVolumeWriteable && radioStatus == 24 && radioPlayDuringKaufman == true) {
					maxVolume = checkMP3PlayerStatus();
				}
				else if (maxVolumeWriteable && radioStatus == 1225 && radioPlayDuringPauseMenu == true) {
					maxVolume = checkMP3PlayerStatus();
				}
				prevRadioStatus = radioStatus;

				if (gameStatus == statuses.Running || gameStatus == statuses.Unconfirmed) {
					try {
						radioStatus = ReadValue(p[0].Handle, address_radio, false, true);
					}
					catch (InvalidOperationException) {
						Debug.WriteLine("InvalidOperationException D");
						gameStatus = statuses.Shutdown;
						return;
					}
					catch (NullReferenceException) {
						Debug.WriteLine("NullReferenceException D");
						gameStatus = statuses.Shutdown;
						return;
					}
					catch (IndexOutOfRangeException) {
						Debug.WriteLine("IndexOutOfRangeException D");
						gameStatus = statuses.Shutdown;
						return;
					}

					volumeStatus = checkMP3PlayerStatus();
					RadioChangerVolume(radioStatus >= 0 && radioStatus <= 9 && radioPlayDuringRadio
										|| radioStatus == 23 && radioPlayDuringEmergency == true
										|| radioStatus == 24 && radioPlayDuringKaufman == true
										|| radioStatus >= 25 && radioStatus <= 26 && radioPlayDuringAnnouncement == true
										|| radioStatus == 1225 && radioPlayDuringPauseMenu == true
					);
				}
			}
		}

		public void CheckRadioStatusSA() {
			if (playerStatus != statuses.Running) {
				return;
			}

			if (actionToTake == actions.Pause) {
				if (gameStatus == statuses.Running || gameStatus == statuses.Unconfirmed) {
					try {
						radioStatus = ReadValue(p[0].Handle, address_radio, false, true);
					}
					catch (InvalidOperationException) {
						Debug.WriteLine("InvalidOperationException H");
						gameStatus = statuses.Shutdown;
						return;
					}
					catch (NullReferenceException) {
						Debug.WriteLine("NullReferenceException H");
						gameStatus = statuses.Shutdown;
						return;
					}
					catch (IndexOutOfRangeException) {
						Debug.WriteLine("IndexOutOfRangeException H");
						gameStatus = statuses.Shutdown;
						return;
					}

					if (radioStatus == 2 && isPaused == true) {
						isPaused = false;
						RadioChangerPause(isPaused);
					}
					else if (radioStatus == 7 && isPaused == false) {
						isPaused = true;
						RadioChangerPause(isPaused);
					}
				}
			}
			else if (actionToTake == actions.Volume) {
				// Unless the radio is currently changing, allow user to change volume
				if (maxVolumeWriteable == true && radioStatus == 2 || gameStatus != statuses.Running && gameStatus != statuses.Unconfirmed) {
					maxVolume = checkMP3PlayerStatus();
				}
				prevRadioStatus = radioStatus;

				if (gameStatus == statuses.Running || gameStatus == statuses.Unconfirmed) {
					try {
						radioStatus = ReadValue(p[0].Handle, address_radio, false, true);
					}
					catch (InvalidOperationException) {
						Debug.WriteLine("InvalidOperationException J");
						gameStatus = statuses.Shutdown;
						return;
					}
					catch (NullReferenceException) {
						Debug.WriteLine("NullReferenceException J");
						gameStatus = statuses.Shutdown;
						return;
					}
					catch (IndexOutOfRangeException) {
						Debug.WriteLine("IndexOutOfRangeException J");
						gameStatus = statuses.Shutdown;
						return;
					}

					volumeStatus = checkMP3PlayerStatus();
					RadioChangerVolume(radioStatus == 2);
				}
			}
		}


		/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * 
				* Media Player Controls
		* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */


		// Change Radio based on pausing/unpausing
		void RadioChangerPause(bool radioOff) {
			radioActive = !radioOff;
			keybd_event(0xB3, 0, 1, IntPtr.Zero);
			keybd_event(0xB3, 0, 2, IntPtr.Zero);
		}

		// Change Radio based on Volume slider
		void RadioChangerVolume(bool radioOn) {
			radioActive = radioOn;
			if (radioOn && volumeStatus < maxVolume) {
				maxVolumeWriteable = false;
				if (Control.ModifierKeys == Keys.Shift || Control.ModifierKeys == Keys.Alt || Control.ModifierKeys == Keys.Control) {
					if (!ignoreMods) {
						return;
					}
				}

				if (volumeStatus == prevVolumeStatus) {
					failSafeAttempts += 1;
					if (failSafeAttempts > 100000) {
						playerStatus = statuses.Error;
						return;
					}
				}
				// radio should be on but volume is too low
				keybd_event(0xAF, 0, 1, IntPtr.Zero);
				if (quickVolume) {
					keybd_event(0xAF, 0, 1, IntPtr.Zero);
					keybd_event(0xAF, 0, 1, IntPtr.Zero);
				}
				keybd_event(0xAF, 0, 2, IntPtr.Zero);
				

				prevVolumeStatus = volumeStatus;
				volumeStatus = checkMP3PlayerStatus();
			}
			else if (!radioOn && volumeStatus > 0) {
				if (Control.ModifierKeys == Keys.Shift || Control.ModifierKeys == Keys.Alt || Control.ModifierKeys == Keys.Control) {
					if (!ignoreMods) {
						return;
					}
				}
				// radio should be off but volume isn't 0
				if (volumeStatus == prevVolumeStatus) {
					failSafeAttempts += 1;
					if (failSafeAttempts > 100000) {
						playerStatus = statuses.Error;
						return;
					}
				}
				keybd_event(0xAE, 0, 1, IntPtr.Zero);
				keybd_event(0xAE, 0, 1, IntPtr.Zero);
				keybd_event(0xAE, 0, 1, IntPtr.Zero);
				if (quickVolume) {
					keybd_event(0xAE, 0, 1, IntPtr.Zero);
					keybd_event(0xAE, 0, 1, IntPtr.Zero);
					keybd_event(0xAE, 0, 1, IntPtr.Zero);
					keybd_event(0xAE, 0, 1, IntPtr.Zero);
					keybd_event(0xAE, 0, 1, IntPtr.Zero);
					keybd_event(0xAE, 0, 1, IntPtr.Zero);
				}
				keybd_event(0xAE, 0, 2, IntPtr.Zero);
				prevVolumeStatus = volumeStatus;
				volumeStatus = checkMP3PlayerStatus();
			}
			else if (maxVolumeWriteable == false) {
				maxVolumeWriteable = true;
				failSafeAttempts = 0;
			}
		}


		// volume slider is used for checking if radio is on or off (winamp didn't want to let me take control of its mute button)
		public int checkMP3PlayerStatus() {
			int volumeLevel = -1;
			if (playerStatus == statuses.Running) {
				try {
					volumeLevel = ReadValue(q[0].Handle, address_volume, musicP == musicPlayers.Foobar, false);
				}
				catch (InvalidOperationException) {
					Debug.WriteLine("InvalidOperationException E");
					playerStatus = statuses.Shutdown;
					return -1;
				}
				catch (NullReferenceException) {
					Debug.WriteLine("NullReferenceException E");
					playerStatus = statuses.Shutdown;
					return -1;
				}
				catch (IndexOutOfRangeException) {
					Debug.WriteLine("IndexOutOfRangeException E");
					playerStatus = statuses.Shutdown;
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
					playerActive = ReadValue(q[0].Handle, address_running, musicP == musicPlayers.Foobar, false);
				}
				catch (InvalidOperationException) {
					Debug.WriteLine("InvalidOperationException F");
					playerStatus = statuses.Shutdown;
					return 0;
				}
				catch (NullReferenceException) {
					Debug.WriteLine("NullReferenceException F");
					playerStatus = statuses.Shutdown;
					return 0;
				}
				catch (IndexOutOfRangeException) {
					Debug.WriteLine("IndexOutOfRangeException F");
					playerStatus = statuses.Shutdown;
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
			if (game == games.SA) {
				CheckRadioStatusSA();
			}
			else if (game == games.VC) {
				CheckRadioStatusVC();
			}
			else if (game == games.III) {
				CheckRadioStatusIII();
			}
		}


		// Bitconverter to return whatever is in that memory address to an integer so I can work with it
		private int ReadValue(IntPtr handle, long address, bool floatRequested, bool fourBytes) {
			if (floatRequested) {
				Single floatInstead = ReadFloat(handle, address);
				return Convert.ToInt32(floatInstead) + 50;
			}
			else if (!fourBytes) {
				return (int)ReadBytes(handle, address, 1)[0];
			}
			else {
				return BitConverter.ToInt32(ReadBytes(handle, address, 4), 0);
			}
		}

		/*private int ReadInt32(IntPtr handle, long address) {
			if (musicP == musicPlayers.Foobar) {
				Single floatInstead = ReadFloat(handle, address);
				return Convert.ToInt32(floatInstead) + 100;
			}
			return BitConverter.ToInt32(ReadBytes(handle, address, 4), 0);
		}*/

		private static Single ReadFloat(IntPtr handle, long address) {
			return BitConverter.ToSingle(ReadBytes(handle, address, 4), 0);
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
