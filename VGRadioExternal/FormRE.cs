using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VGRadioExternal {
    public partial class FormRE : Form {

        static string inputPath;

        Radio radio;
        // the file each list has loaded, to go back to when a newly picked one can't be used
        string gameFile;
        string musicPlayerFile;
        bool reverting;

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        public FormRE() {
            InitializeComponent();

            inputPath = Path.Combine(Environment.CurrentDirectory, "input");

            //Process process = WinApi.GetProcess("wmplayer");
            //int nRet;
            //StringBuilder ClassName = new StringBuilder(100);
            //nRet = GetClassName(process.MainWindowHandle, ClassName, ClassName.Capacity);
            //string a = ClassName.ToString();
            //Debug.WriteLine(a);
            //int _window = WinApi.FindWindow("WMPlayerApp", null);
            //WinApi.SendMessage(_window, 0x111, 0x4981, 0);
            radio = new Radio();
        }

        private void FormRE_Load(object sender, EventArgs e) {
            RefreshLists();
            radio.StatusChanged += Radio_StatusChanged;
            radio.StartTimer();
        }

        private void FormRE_FormClosing(object sender, FormClosingEventArgs e) {
            // leave the music player playing
            radio.Stop();
        }

        /// <summary>
        /// Shows the status. Radio raises this on the timer thread, so the labels are set through BeginInvoke.
        /// </summary>
        private void Radio_StatusChanged(string game, string musicPlayer) {
            BeginInvoke((Action)(() => {
                Label_StatusGame.Text = game;
                Label_StatusMusicPlayer.Text = musicPlayer;
            }));
        }

        private void RefreshLists() {
            ComboBox_Game.Items.Clear();
            string[] games = Directory.GetFiles(Path.Combine(inputPath, "games"));
            foreach (string game in games) {
                ComboBox_Game.Items.Add(Path.GetFileName(game));
			}
            
            ComboBox_MusicPlayer.Items.Clear();
            string[] musicPlayers = Directory.GetFiles(Path.Combine(inputPath, "music players"));
            foreach (string mp in musicPlayers) {
                ComboBox_MusicPlayer.Items.Add(Path.GetFileName(mp));
            }
        }

		private void Button_OpenDirectory_Click(object sender, EventArgs e) {
            Process.Start(inputPath);
		}

		private void Button_Refresh_Click(object sender, EventArgs e) {
            RefreshLists();
		}

		private void ComboBox_MusicPlayer_SelectedIndexChanged(object sender, EventArgs e) {
            musicPlayerFile = LoadPicked(ComboBox_MusicPlayer, musicPlayerFile, "music players", radio.MusicPlayerChanged);
		}

		private void ComboBox_Game_SelectedIndexChanged(object sender, EventArgs e) {
            gameFile = LoadPicked(ComboBox_Game, gameFile, "games", radio.GameChanged);
        }

        /// <summary>
        /// Loads the file picked in a list. If it can't be used, says why and picks the loaded file again. Returns the file that's loaded.
        /// </summary>
        string LoadPicked(ComboBox list, string loadedFile, string folder, Action<string> load) {
            if (reverting || list.SelectedIndex < 0) {
                return loadedFile;
            }
            string file = (string)list.SelectedItem;
            try {
                load(Path.Combine(inputPath, folder, file));
                return file;
            }
            catch (InvalidDataException ex) {
                MessageBox.Show(this, file + " can't be used.\n\n" + ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                // the loaded file is still in use, so the list shows it again. Picking it this way doesn't load it again
                reverting = true;
                try {
                    list.SelectedIndex = loadedFile == null ? -1 : list.Items.IndexOf(loadedFile);
                }
                finally {
                    reverting = false;
                }
                return loadedFile;
            }
        }
    }
}
