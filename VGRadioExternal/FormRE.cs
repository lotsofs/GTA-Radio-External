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
            string path = Path.Combine(inputPath, "music players", ComboBox_MusicPlayer.Text);
            radio.MusicPlayerChanged(path);
		}

		private void ComboBox_Game_SelectedIndexChanged(object sender, EventArgs e) {
            string path = Path.Combine(inputPath, "games", ComboBox_Game.Text);
            radio.GameChanged(path);
        }
    }
}
