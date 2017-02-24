using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

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

			// Check if the music player still exists
			if (radioButtonWinamp.Checked) {
				readMemory.DeterminePlayerVersionWinamp();
			}
			else if (radioButtonOther.Checked) {
				readMemory.DeterminePlayerVersionOther();
			}
		}


		void UpdateWindow() {
			if (readMemory.actionToTake == ReadMemory.actions.None) {
				label1.Text = "Tool not configured";
				displayedText = displayedTexts.Unitialized;
			}
			else if (readMemory.playerStatus == ReadMemory.statuses.Shutdown) {
				label1.Text = "Music player not running";
				displayedText = displayedTexts.NoMusicPlayer;
			}
			else if (readMemory.gameStatus == ReadMemory.statuses.Shutdown && displayedText != displayedTexts.Shutdown) {
				label1.Text = "Game not running";
				displayedText = displayedTexts.Shutdown;
			}
			else if (readMemory.gameStatus == ReadMemory.statuses.Unrecognized && displayedText != displayedTexts.Unrecognized) {
				label1.Text = "Unable to detect game version";
				displayedText = displayedTexts.Unrecognized;
			}
			else if (readMemory.gameStatus == ReadMemory.statuses.Unconfirmed) {
				if (readMemory.radioStatus == 2) {
					label1.Text = "radio ON (or at least it should be)";
					//displayedText = displayedTexts.Unconfirmed;
				}
				else if (readMemory.radioStatus == 7) {
					label1.Text = "radio OFF (or at least it should be)";
					//displayedText = displayedTexts.Unconfirmed;
				}
				else {
					label1.Text = "Whoopsee, something went wrong. Here's a number: " + readMemory.radioStatus;
				}
			}
			else if (readMemory.gameStatus == ReadMemory.statuses.Running) {
				if (readMemory.radioStatus == 2) {
					label1.Text = "radio ON";
					//displayedText = displayedTexts.Running;
				}
				else if (readMemory.radioStatus == 7) {
					label1.Text = "radio OFF";
					//displayedText = displayedTexts.Running;
				}
			}
			if (radioButtonWinamp.Checked) {
				labelVolume.Text = "Volume: " + readMemory.maxVolume;
			}
			else {
				labelVolume.Text = null;
			}
			label2.Text = "V" + readMemory.major + ".0" + readMemory.minor + " " + readMemory.region;
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

		private void checkBox1_CheckedChanged(object sender, EventArgs e) {
			readMemory.quickVolume = checkBox1.Checked;
		}

		private void radioButtonVolume_CheckedChanged(object sender, EventArgs e) {
			readMemory.actionToTake = ReadMemory.actions.Volume;
			checkBox1.Enabled = radioButtonVolume.Checked;
		}

		private void radioButtonPause_CheckedChanged(object sender, EventArgs e) {
			if (radioButtonPause.Checked) {
				readMemory.actionToTake = ReadMemory.actions.Pause;
			}
		}

		private void radioButtonOther_CheckedChanged(object sender, EventArgs e) {
			radioButtonVolume.Enabled = !radioButtonOther.Checked;
			radioButtonMute.Enabled = !radioButtonOther.Checked;
			radioButtonPause.Enabled = radioButtonOther.Checked;
		}

		private void radioButtonWinamp_CheckedChanged(object sender, EventArgs e) {
			radioButtonVolume.Enabled = radioButtonWinamp.Checked;
			radioButtonMute.Enabled = !radioButtonWinamp.Checked;
			radioButtonPause.Enabled = radioButtonWinamp.Checked;
		}

		private void radioButtonVolume_EnabledChanged(object sender, EventArgs e) {
			if (radioButtonVolume.Checked) {
				radioButtonVolume.Checked = false;
				readMemory.actionToTake = ReadMemory.actions.None;
				radioButtonPause.Checked = true;
			}
		}
	}
}
