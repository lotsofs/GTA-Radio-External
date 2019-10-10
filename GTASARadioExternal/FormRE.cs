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

namespace GTASARadioExternal {
    public partial class FormRE : Form {

        Radio radio;
        
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        public FormRE() {
            InitializeComponent();

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

        }

        private void label5_Click(object sender, EventArgs e) {

        }
    }
}
