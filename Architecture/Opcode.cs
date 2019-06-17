using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Architecture
{
    internal struct Opcode
    {
        private ushort fullCode;
        private ushort firstNibble;
        private byte x;
        private byte y;
        private byte kk;
        private byte n;
        private ushort nnn;

        public ushort FullCode
        {
            get { return fullCode; }
        }
        public ushort FirstNibble
        {
            get { return firstNibble; }
        }
        public byte X
        {
            get { return x; }
        }
        public byte Y
        {
            get { return y; }
        }
        public byte KK
        {
            get { return kk; }
        }
        public byte N
        {
            get { return n; }
        }
        public ushort NNN
        {
            get { return nnn; }
        }
        public Opcode(ushort opcode)
        {
            fullCode = opcode;
            firstNibble = (ushort)(opcode & 0xF000);
            x = (byte)((opcode & 0x0F00) >> 8);
            y = (byte)((opcode & 0x00F0) >> 4);
            kk = (byte)((opcode & 0x00FF));
            n = (byte)((opcode & 0x000F));
            nnn = (ushort)((opcode & 0x0FFF));
        }

    }
}
