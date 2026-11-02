using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VGRadioExternal {

    class MPData {
        public string Description;
        public string ProcessName;

        public int MinVolume;

        public ReadVolume ReadVolume;
        public SetVolume SetVolume;
    }

    class ReadVolume {
        public string Module;
        public int AddressOffset;
        public string Type;
    }

    class SetVolume {
        public HookMethods Type;
        public string WindowName;
        public string WMsg;
        public string LParam;
        public string WParam;
        public string Arguments;
        public string Keys;
    }

    public enum HookMethods {
        None,
        SendMessage,    // for winamp
        ProcessStart,    // for foobar
        MemoryWrite,     // for anything else?
        AudioSession,    // for spotify, or any other app in the volume mixer
        KeyPress    // for any player with a global mute hotkey
    }

}
