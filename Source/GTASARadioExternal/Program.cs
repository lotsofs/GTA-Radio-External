using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;




namespace GTASARadioExternal {
    static class Program {

        /*static Timer timer1;
        public static int radioStatus = 9;        // 7 = inaudible , 2 = audible , everything else can be ignored
        public static int prevRadioStatus = 9;
        public static int volumeStatus;        // 0 - 255 because I couldnt find a way to reliably mute stuff that I could understand
        public static int maxVolume = 20;

		

        // We need to read memory from GTA so we need to import some code from this DLL.
        const int PROCESS_WM_READ = 0x0010;

        [DllImport("kernel32.dll")]
        public static extern Int32 ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress,
          [In, Out] byte[] buffer, UInt32 size, out IntPtr lpNumberOfBytesRead);

        // Send keystrokes for volume
        [DllImport("user32.dll")]
        public static extern void keybd_event(byte virtualKey, byte scanCode, uint flags, IntPtr extraInfo);

		*/


        // Application launched
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

		/*// Determine the version of GTASA
		public static void determineGameVersion() {
			Process[] p = Process.GetProcessesByName("gta-sa");
			if (p.Length == 0) {
				p = Process.GetProcessesByName("gta_sa");
			}
			if (p.Length == 0) {
				radioStatus = -2;
				// error, game not running
			}
			
			//int aaaa = ReadInt32(p[0].Handle, add_volume);
		}


        private static void checkRadioStatus() {
            int add_base;                   // The base ADDress of gtasa
            int add_radio = 0x0053AB68;     // The address of the int that changes depending on radio status (v3.0 version of GTASA)

            Process[] p = Process.GetProcessesByName("gta-sa");
            // unless the radio station has just been switched on a frame ago, allow the user to adjust the volume.
            if (prevRadioStatus == radioStatus && radioStatus == 2) {
                maxVolume = checkMP3PlayerStatus();
            }
            prevRadioStatus = radioStatus;

            if (p.Length != 0) {
                add_base = p[0].MainModule.BaseAddress.ToInt32();
                add_radio += add_base;

                radioStatus = ReadInt32(p[0].Handle, add_radio);       // Set this int to be the same as the address in the game ( 2 or 7 )
                volumeStatus = checkMP3PlayerStatus();

                if (radioStatus == 2 && volumeStatus < maxVolume) {
                    // radio is supposed to be on but the volume is 0
                    while (volumeStatus < maxVolume) {
                        keybd_event(0xAF, 0, 1, IntPtr.Zero);   // Volume up
                        volumeStatus = checkMP3PlayerStatus();
                    }
                    keybd_event(0xAF , 0, 2, IntPtr.Zero);
                }
                else if (radioStatus == 7 && volumeStatus > 0) {
					// radio is supposed to be off but the volume is not at 0
					for (int i = 0; i < 50; i++) {
						keybd_event(0xAE, 0, 1, IntPtr.Zero); // volume down
						volumeStatus = checkMP3PlayerStatus();
						if (volumeStatus >= checkMP3PlayerStatus()) {
							i = 255;
						}
					}
                    keybd_event(0xAE, 0, 2, IntPtr.Zero);
                }
                else if (volumeStatus == -1) {
                    radioStatus = -1;       // winamp not running
                }
            }
            else {
                radioStatus = -2;           // gtasa not running
            }
        }

        // This function checks whether winamp is actually playing. To prevent crash if winamp has just been booted but hasn't started music yet.
        private static int checkMP3ActiveStatus() {
            int add_base;
            int add_running = 0xBF3EC;
            int playerActive = -1;

            Process[] p = Process.GetProcessesByName("winamp");
            if (p.Length != 0) {
                add_base = p[0].MainModule.BaseAddress.ToInt32();
                add_running += add_base;

                playerActive = ReadInt32(p[0].Handle, add_running);
                return playerActive;
            }
            else {
                return -1;
            }
        }

        // volume slider is used for checking if radio is on or off (winamp didn't want to let me take control of its mute button)
        private static int checkMP3PlayerStatus() {
            int add_base;                   // The base ADDress of winamp
            int add_volume = 0x076A9D9C;    // volume slider address
            int volumeLevel = -1;

            Process[] p = Process.GetProcessesByName("winamp");
            if (p.Length != 0) {            // Gotta make sure the app is running or it'll throw an error

                add_base = p[0].MainModule.BaseAddress.ToInt32();
                add_volume += add_base;

                volumeLevel = ReadInt32(p[0].Handle, add_volume);       // Set this int to be the same as the address in the game ( 2 or 7 )
                if (volumeLevel == 255) {
                    int activity;
                    activity = checkMP3ActiveStatus();          // Returns 1 if player is running, 0 if not. Multiplying returns 0 if player is dead to avoid constant muting when winamp just launched.
                    return volumeLevel * activity;
                }
                return volumeLevel;
            }
            else {
                return -1;
            }
        }

        private static int ReadInt32(IntPtr handle, long address) {
            return BitConverter.ToInt32(ReadBytes(handle, address, 4), 0);      // Bitconverter to return whatever is in that memory address to an integer so I can work with it
        }

        private static byte[] ReadBytes(IntPtr handle, long address, uint bytesToRead) {
            IntPtr ptrBytesRead;                    // I have no idea what this means but anyway this is where the memory is read and it returns the value, but it's not an int?
            byte[] buffer = new byte[bytesToRead];
            ReadProcessMemory(handle, new IntPtr(address), buffer, bytesToRead, out ptrBytesRead);
            return buffer;
        }

        // timer meuk to make it check the entire time instead of just at launch. simple
        public static void InitTimer() {
            timer1 = new Timer();
            timer1.Tick += new EventHandler(timer1Tick);
            timer1.Interval = 40;
            timer1.Start();
        }

        static void timer1Tick(object sender, EventArgs e) {
			checkRadioStatus();
        }*/

    }
}
