using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTASARadioExternal {
    public class Module {   // todo: FRAMEWORK
        public Module(string moduleName, IntPtr baseAddress, uint size) {
            ModuleName = moduleName;
            BaseAddress = baseAddress;
            Size = size;
        }
        public string ModuleName;
        public IntPtr BaseAddress;
        public uint Size;
    }
}
