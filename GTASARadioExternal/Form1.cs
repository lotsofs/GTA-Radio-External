﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Configuration;

namespace GTASARadioExternal {
	public partial class Form1 : Form {

		Timer timer2;

		public enum displayedTexts { Unitialized, Shutdown, Running, Unrecognized, Unconfirmed, NoMusicPlayer }
		displayedTexts displayedText = displayedTexts.Unitialized;

		public static ReadMemory readMemory = new ReadMemory();


		public Form1() {
			InitializeComponent();

			label1.Text = "Tool not configured";

			readMemory.InitTimer();        // run the timer that checks for updates
			WindowTimer();              // run the timer that prints these updates
		}

		void CheckGame() {
			// Check if the game still exists
			if (radioButtonSA.Checked) {
				readMemory.DetermineGameVersionSA();
			}
			else if (radioButtonVC.Checked) {
				readMemory.DetermineGameVersionVC();
			}
			else if (radioButtonIII.Checked) {
				readMemory.DetermineGameVersionIII();
			}
			else if (radioButtonSR3.Checked) {
				readMemory.DetermineGameVersionSR3();
			}

			// Check if the music player still exists
			if (radioButtonWinamp.Checked) {
				readMemory.DeterminePlayerVersionWinamp();
			}
			if (radioButtonFoobar.Checked) {
				readMemory.DeterminePlayerVersionFoobar();
			}
			else if (radioButtonOther.Checked) {
				readMemory.DeterminePlayerVersionOther();
			}
		}


		void UpdateWindow() {
			if (readMemory.actionToTake == ReadMemory.actions.None) {
				label1.Text = "Tool not configured";
				label2.Text = "V" + readMemory.major + "." + readMemory.minor + " " + readMemory.region;
				displayedText = displayedTexts.Unitialized;
			}
			else if (readMemory.playerStatus == ReadMemory.statuses.Shutdown) {
				label1.Text = "Music player not running";
				label2.Text = "V" + readMemory.major + "." + readMemory.minor + " " + readMemory.region;
				displayedText = displayedTexts.NoMusicPlayer;
			}
			else if (readMemory.playerStatus == ReadMemory.statuses.Error) {
				label1.Text = "ERROR";
				label2.Text = "V" + readMemory.major + "." + readMemory.minor + " " + readMemory.region;
			}
			else if (readMemory.gameStatus == ReadMemory.statuses.Shutdown && displayedText != displayedTexts.Shutdown) {
				label1.Text = "Game not running";
				label2.Text = "";
				displayedText = displayedTexts.Shutdown;
			}
			else if (readMemory.gameStatus == ReadMemory.statuses.Unrecognized && displayedText != displayedTexts.Unrecognized) {
				label1.Text = "Unable to detect game version";
				label2.Text = "V" + readMemory.major + "." + readMemory.minor + " " + readMemory.region;
				displayedText = displayedTexts.Unrecognized;
			}
			else if (readMemory.gameStatus == ReadMemory.statuses.Unconfirmed) {
				if (readMemory.radioActive) {
					label1.Text = "radio ON (I think)";
					label2.Text = "V" + readMemory.major + "." + readMemory.minor + " " + readMemory.region;
					//displayedText = displayedTexts.Unconfirmed;
				}
				else {
					label1.Text = "radio OFF (I think)";
					label2.Text = "V" + readMemory.major + "." + readMemory.minor + " " + readMemory.region;
					//displayedText = displayedTexts.Unconfirmed;
				}
			}
			else if (readMemory.gameStatus == ReadMemory.statuses.Running) {
				if (readMemory.radioActive) {
					label1.Text = "radio ON";
					label2.Text = "V" + readMemory.major + "." + readMemory.minor + " " + readMemory.region;
					//displayedText = displayedTexts.Running;
				}
				else {
					label1.Text = "radio OFF";
					label2.Text = "V" + readMemory.major + "." + readMemory.minor + " " + readMemory.region;
					//displayedText = displayedTexts.Running;
				}
			}
			if (radioButtonWinamp.Checked || radioButtonFoobar.Checked) {
				labelVolume.Text = "Volume: " + readMemory.maxVolume;
			}
			else {
				labelVolume.Text = null;
			}
		}

		// print some status stuff
		/*void UpdateWindowOld() {
            if (Program.radioStatus == 2) {
                label1.Text = "Radio ON with volume " + Program.volumeStatus;
                label2.Text = Program.volumeStatus.ToString();
                displayedText = 2;
            }
            else if (Program.radioStatus == 7 && displayedText != 7) {
                label1.Text = "Radio OFF";
                displayedText = 7;
            }
            else if (Program.radioStatus == -1 && displayedText != -1) {
                label1.Text = "Winamp not running";
                displayedText = -1;
            }
            else if (Program.radioStatus == -2 && displayedText != -2) {
                label1.Text = "GTASA not running";
                displayedText = -2;
            }
        }*/

		// timer that checks every second for updates to be made to the printed info on the window
		public void WindowTimer() {
			timer2 = new Timer();
			timer2.Tick += new EventHandler(timer2Tick);
			timer2.Interval = 1000;
			timer2.Start();
		}

		void timer2Tick(object sender, EventArgs e) {
			UpdateWindow();
			CheckGame();
		}

		#region unsorted list music players + action buttons
		private void checkBox1_CheckedChanged(object sender, EventArgs e) {
			readMemory.quickVolume = checkBox1.Checked;
			readMemory.maxVolumeWriteable = false;
		}

		private void radioButtonVolume_CheckedChanged(object sender, EventArgs e) {
			readMemory.actionToTake = ReadMemory.actions.Volume;
			checkBox1.Enabled = radioButtonVolume.Checked;
			readMemory.maxVolumeWriteable = false;
			checkBox7.Enabled = radioButtonVolume.Checked;
		}

		private void radioButtonPause_CheckedChanged(object sender, EventArgs e) {
			readMemory.isPaused = false;
			if (radioButtonPause.Checked) {
				readMemory.actionToTake = ReadMemory.actions.Pause;
			}
			readMemory.maxVolumeWriteable = false;
		}

		private void radioButtonMute_CheckedChanged(object sender, EventArgs e) {
			readMemory.isMuted = false;
			if (radioButtonMute.Checked) {
				readMemory.actionToTake = ReadMemory.actions.Mute;
			}
			readMemory.maxVolumeWriteable = false;
		}

		private void radioButtonOther_CheckedChanged(object sender, EventArgs e) {
			if (radioButtonOther.Checked) {
				readMemory.musicP = ReadMemory.musicPlayers.Other;
				radioButtonVolume.Enabled = !radioButtonOther.Checked;
				radioButtonMute.Enabled = !radioButtonOther.Checked;
				radioButtonPause.Enabled = radioButtonOther.Checked;
				readMemory.maxVolumeWriteable = false;
			}
		}

		private void radioButtonWinamp_CheckedChanged(object sender, EventArgs e) {
			if (radioButtonWinamp.Checked) {
				radioButtonVolume.Enabled = radioButtonWinamp.Checked;
				radioButtonMute.Enabled = radioButtonWinamp.Checked;
				radioButtonPause.Enabled = radioButtonWinamp.Checked;
				readMemory.musicP = ReadMemory.musicPlayers.Winamp;
				readMemory.maxVolumeWriteable = false;
				readMemory.DeterminePlayerVersionWinamp();
				readMemory.maxVolume = readMemory.checkMP3PlayerStatus();
			}
		}

		private void radioButtonFoobar_CheckedChanged(object sender, EventArgs e) {
			if (radioButtonFoobar.Checked) {
				radioButtonVolume.Enabled = radioButtonFoobar.Checked;
				radioButtonMute.Enabled = radioButtonFoobar.Checked;
				radioButtonPause.Enabled = radioButtonFoobar.Checked;
				readMemory.musicP = ReadMemory.musicPlayers.Foobar;
				readMemory.maxVolumeWriteable = false;
				readMemory.DeterminePlayerVersionFoobar();
				readMemory.maxVolume = readMemory.checkMP3PlayerStatus();
			}
		}


		private void radioButtonVolume_EnabledChanged(object sender, EventArgs e) {
			if (radioButtonVolume.Checked) {
				radioButtonVolume.Checked = false;
				readMemory.actionToTake = ReadMemory.actions.None;
				radioButtonPause.Checked = true;
				readMemory.DeterminePlayerVersionOther();
			}
		}
		#endregion

		#region game radio buttons
		private void radioButtonIII_CheckedChanged(object sender, EventArgs e) {
			readMemory.DetermineGameVersionIII();
			readMemory.game = ReadMemory.games.III;
			checkBoxA.Enabled = true;
			checkBoxA.Checked = true;
			checkBoxB.Enabled = true;
			checkBoxB.Checked = true;
			checkBoxC.Enabled = false;
			checkBoxC.Checked = false;
			checkBoxD.Enabled = true;
			//checkBoxD.Checked = false;
			checkBoxE.Enabled = false;
			checkBoxE.Checked = false;
			checkBoxF.Enabled = true;
			checkBoxF.Checked = false;
			readMemory.maxVolumeWriteable = false;
			readMemory.p = null;
			readMemory.gameStatus = ReadMemory.statuses.Shutdown;
		}

		private void radioButtonVC_CheckedChanged(object sender, EventArgs e) {
			readMemory.DetermineGameVersionVC();
			readMemory.game = ReadMemory.games.VC;
			checkBoxA.Enabled = true;
			checkBoxA.Checked = true;
			checkBoxB.Enabled = true;
			checkBoxB.Checked = true;
			checkBoxC.Enabled = false;
			checkBoxC.Checked = false;
			checkBoxD.Enabled = true;
			//checkBoxD.Checked = false;
			checkBoxE.Enabled = true;
			checkBoxE.Checked = true;
			checkBoxF.Enabled = true;
			checkBoxF.Checked = false;
			readMemory.maxVolumeWriteable = false;
			readMemory.p = null;
			readMemory.gameStatus = ReadMemory.statuses.Shutdown;
		}

		private void radioButtonSA_CheckedChanged(object sender, EventArgs e) {
			readMemory.DetermineGameVersionSA();
			readMemory.game = ReadMemory.games.SA;
			checkBoxA.Enabled = false;
			checkBoxA.Checked = true;
			checkBoxB.Enabled = false;
			checkBoxB.Checked = true;
			checkBoxC.Enabled = false;
			checkBoxC.Checked = true;
			checkBoxD.Enabled = false;
			checkBoxD.Checked = true;
			checkBoxE.Enabled = false;
			checkBoxE.Checked = false;
			checkBoxF.Enabled = false;
			checkBoxF.Checked = false;
			readMemory.maxVolumeWriteable = false;
			readMemory.p = null;
			readMemory.gameStatus = ReadMemory.statuses.Shutdown;
		}

		private void radioButtonSR3_CheckedChanged(object sender, EventArgs e) {
			readMemory.DetermineGameVersionSR3();
			readMemory.game = ReadMemory.games.SR3;
			checkBoxA.Enabled = false;
			checkBoxA.Checked = true;
			checkBoxB.Enabled = false;
			checkBoxB.Checked = true;
			checkBoxC.Enabled = false;
			checkBoxC.Checked = false;
			checkBoxD.Enabled = false;
			checkBoxD.Checked = true;
			checkBoxE.Enabled = false;
			checkBoxE.Checked = false;
			checkBoxF.Enabled = false;
			checkBoxF.Checked = false;
			readMemory.maxVolumeWriteable = false;
			readMemory.p = null;
			readMemory.gameStatus = ReadMemory.statuses.Shutdown;
		}

		#endregion

		#region when buttons
		private void checkBoxA_CheckedChanged(object sender, EventArgs e) {
			readMemory.radioPlayDuringEmergency = checkBoxA.Checked;
			readMemory.maxVolumeWriteable = false;
		}

		private void checkBoxB_CheckedChanged(object sender, EventArgs e) {
			readMemory.radioPlayDuringRadio = checkBoxB.Checked;
			readMemory.maxVolumeWriteable = false;
		}

		private void checkBoxC_CheckedChanged(object sender, EventArgs e) {

		}

		private void checkBoxD_CheckedChanged(object sender, EventArgs e) {
			readMemory.radioPlayDuringPauseMenu = checkBoxD.Checked;
			readMemory.maxVolumeWriteable = false;
		}

		private void checkBoxE_CheckedChanged(object sender, EventArgs e) {
			readMemory.radioPlayDuringKaufman = checkBoxE.Checked;
			readMemory.maxVolumeWriteable = false;
		}

		private void checkBoxF_CheckedChanged(object sender, EventArgs e) {
			readMemory.radioPlayDuringAnnouncement = checkBoxF.Checked;
			readMemory.maxVolumeWriteable = false;
		}

		private void checkBox7_CheckedChanged(object sender, EventArgs e) {
			readMemory.ignoreMods = checkBox7.Checked;
		}


		#endregion

		

		#region configuration

		private void Form1_FormClosing(object sender, FormClosingEventArgs e) {

			try {
				Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

				//game
				if (radioButtonSA.Checked) {
					config.AppSettings.Settings["gameSet"].Value = "SA";
				}
				else if (radioButtonVC.Checked) {
					config.AppSettings.Settings["gameSet"].Value = "VC";
				}
				else if (radioButtonIII.Checked) {
					config.AppSettings.Settings["gameSet"].Value = "III";
				}
				else if (radioButtonSR3.Checked) {
					config.AppSettings.Settings["gameSet"].Value = "SR3";
				}
				//player
				if (radioButtonWinamp.Checked) {
					config.AppSettings.Settings["playerSet"].Value = "Winamp";
				}
				else if (radioButtonFoobar.Checked) {
					config.AppSettings.Settings["playerSet"].Value = "Foobar";
				}
				else if (radioButtonVolume.Checked) {
					config.AppSettings.Settings["playerSet"].Value = "Other";
				}
				//action
				if (radioButtonMute.Checked) {
					config.AppSettings.Settings["actionSet"].Value = "Mute";
				}
				else if (radioButtonPause.Checked) {
					config.AppSettings.Settings["actionSet"].Value = "Pause";
				}
				else if (radioButtonVolume.Checked) {
					config.AppSettings.Settings["actionSet"].Value = "Volume";
				}
				//action-settings
				config.AppSettings.Settings["quickvolumeSet"].Value = checkBox1.Checked.ToString();
				config.AppSettings.Settings["ignoremodifiersSet"].Value = checkBox7.Checked.ToString();
				//when
				config.AppSettings.Settings["emergencySet"].Value = checkBoxA.Checked.ToString();
				config.AppSettings.Settings["radioSet"].Value = checkBoxB.Checked.ToString();
				config.AppSettings.Settings["interiorsSet"].Value = checkBoxC.Checked.ToString();
				config.AppSettings.Settings["announcerSet"].Value = checkBoxF.Checked.ToString();
				config.AppSettings.Settings["kaufmanSet"].Value = checkBoxE.Checked.ToString();
				config.AppSettings.Settings["menuSet"].Value = checkBoxD.Checked.ToString();

				config.Save(ConfigurationSaveMode.Modified);
			}
			catch (NullReferenceException) {
				Debug.WriteLine("Error writing app settings");
			}
		}

		private void Form1_Load(object sender, EventArgs e) {
			try {
				switch (ConfigurationManager.AppSettings["gameSet"]) {
					case "SA":
						radioButtonSA.Checked = true;
						break;
					case "VC":
						radioButtonVC.Checked = true;
						break;
					case "III":
						radioButtonIII.Checked = true;
						break;
					case "SR3":
						radioButtonSR3.Checked = true;
						break;
					default:
						break;
				}
				switch (ConfigurationManager.AppSettings["playerSet"]) {
					case "Winamp":
						radioButtonWinamp.Checked = true;
						break;
					case "Foobar":
						radioButtonFoobar.Checked = true;
						break;
					case "Other":
						radioButtonOther.Checked = true;
						break;
					default:
						break;
				}
				switch (ConfigurationManager.AppSettings["actionSet"]) {
					case "Mute":
						radioButtonMute.Checked = true;
						break;
					case "Pause":
						radioButtonPause.Checked = true;
						break;
					case "Volume":
						radioButtonVolume.Checked = true;
						break;
					default:
						break;
				}
				checkBox1.Checked = bool.Parse(ConfigurationManager.AppSettings["quickvolumeSet"]);
				checkBox7.Checked = bool.Parse(ConfigurationManager.AppSettings["ignoremodifiersSet"]);
				checkBoxA.Checked = bool.Parse(ConfigurationManager.AppSettings["emergencySet"]);
				checkBoxB.Checked = bool.Parse(ConfigurationManager.AppSettings["radioSet"]);
				checkBoxC.Checked = bool.Parse(ConfigurationManager.AppSettings["interiorsSet"]);
				checkBoxF.Checked = bool.Parse(ConfigurationManager.AppSettings["announcerSet"]);
				checkBoxE.Checked = bool.Parse(ConfigurationManager.AppSettings["kaufmanSet"]);
				checkBoxD.Checked = bool.Parse(ConfigurationManager.AppSettings["menuSet"]);
			}

			catch (NullReferenceException) {
				Debug.WriteLine("Error reading app settings");
			}
		}






		#endregion


	}
}
