using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Architecture
{
    public class CPU
    {
        //General purpose 8-bit registers, referred to as Vx, where x is a hexadecimal digit (0 through F)
        public byte[] V = new byte[16];

        //16-bit register called I. This register is generally used to store memory addresses, so only the lowest (rightmost) 12 bits are usually used.
        public ushort I;

        /*Chip-8 also has two special purpose 8-bit registers, for the delay and sound timers. 
          When these registers are non-zero, they are automatically decremented at a rate of 60Hz.*/
        public byte DelayTimer;
        public byte SoundTimer;

        //The program counter(PC) is 16-bit, and is used to store the currently executing address
        public short PC;

        //The stack pointer (SP) is 8-bit, it is used to point to the topmost level of the stack.
        public byte SP;

        public ushort[] Stack = new ushort[16];

        public Display Display = new Display();

        public void ExecuteOpcode(ushort opcode)
        {
            ushort nibble = (ushort)(opcode & 0xF000);
            Console.WriteLine($"{nibble:X}");
        }

    }
}
