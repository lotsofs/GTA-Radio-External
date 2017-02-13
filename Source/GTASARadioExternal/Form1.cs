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
        int displayedText;

        public Form1() {
            InitializeComponent();
            Program.InitTimer();        // run the timer that checks for updates
            WindowTimer();              // run the timer that prints these updates
        }

        private void label1_Click(object sender, EventArgs e) {
            // Unused but removing it throws an error :D
        }

        // print some status stuff
        void updateWindow() {
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
        }

        // timer that checks every second for updates to be made to the printed info on the window
        public void WindowTimer() {
            timer2 = new Timer();
            timer2.Tick += new EventHandler(timer2Tick);
            timer2.Interval = 1000;
            timer2.Start();
        }

        void timer2Tick(object sender, EventArgs e) {
            updateWindow();
        }

        // pointless feature I added where you could enter the max volume in a box, but then made it controllable via winamp itself anyway
        private void button1_Click(object sender, EventArgs e) {
            /*int outThingIDontNeed;
            if (Int32.TryParse(textBox1.Text, out outThingIDontNeed)) {
                if (Int32.Parse(textBox1.Text) <= 255 && Int32.Parse(textBox1.Text) > 0) {
                    Program.maxVolume = Int32.Parse(textBox1.Text);
                    label2.Text = Int32.Parse(textBox1.Text).ToString();
                }
            }*/
        }

        private void textBox1_TextChanged(object sender, EventArgs e) {
            // Unused but removing it throws an error :D
        }

        private void label2_Click(object sender, EventArgs e) {
            // Unused but removing it throws an error :D
        }

        private void Form1_Load(object sender, EventArgs e) {
            // Unused but removing it throws an error :D
        }

        private void label3_Click(object sender, EventArgs e) {
            // Unused but removing it throws an error :D
        }
    }
}
