using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Architecture
{
    /// <summary>
    /// Opcode structure provides
    /// easy access to required 
    /// instruction data
    /// </summary>
    internal struct Opcode
    {
        public ushort FullCode { get; }
        public ushort FirstNibble { get; }
        public byte X { get; }
        public byte Y { get; }
        public byte KK { get; }
        public byte N { get; }
        public ushort NNN { get; }
        public Opcode(ushort opcode)
        {
            FullCode = opcode;
            FirstNibble = (ushort)(opcode & 0xF000);
            X = (byte)((opcode & 0x0F00) >> 8);
            Y = (byte)((opcode & 0x00F0) >> 4);
            KK = (byte)((opcode & 0x00FF));
            N = (byte)((opcode & 0x000F));
            NNN = (ushort)((opcode & 0x0FFF));
        }

    }
}
