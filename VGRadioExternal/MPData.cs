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
    }

    public enum HookMethods {
        None,
        SendMessage,    // for winamp, spotify
        ProcessStart,    // for foobar
        MemoryWrite     // for anything else?
    }

}
