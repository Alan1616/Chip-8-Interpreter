using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Architecture
{
    public partial class CPU : IDircectKeyboardAccess
    {
        // Set of opcodes dictionaries pointing to
        // certain opcode or a method that does 
        // lookup and finds opcode.
        private readonly Dictionary<ushort, Action<Opcode>> MainOpcodeMap;
        private readonly Dictionary<ushort, Action<Opcode>> ArithmeticsOpcodeMap;
        private readonly Dictionary<ushort, Action<Opcode>> LoadsOpcodeMap;
        // Should cpu stop exceution waiting for keypress.
        public bool AwaitsForKeypress { get; set; } = false;
        // Event that is invoked when chip-8 waits for keypress.
        // Should be subscribed by your keyboard access class instance.
        public event EventHandler<bool> WaitForKeypressEvent;

        /// <summary>
        /// Default clock rate Chip-8 runs at.
        /// </summary>
        public int CPUClockRate { get; set; } = 600;

        // General purpose 8-bit registers, referred to as Vx, where x is a hexadecimal digit (0 through F)
        private byte[] V = new byte[16];
        // 16-bit register called I. This register is generally used to store memory addresses, so only the lowest (rightmost) 12 bits are usually used.
        private ushort I;
        /*Chip-8 also has two special purpose 8-bit registers, for the delay and sound timers. 
          When these registers are non-zero, they are automatically decremented at a rate of 60Hz.*/
        private byte DelayTimer;
        private byte SoundTimer;

        //The program counter(PC) is 16-bit, and is used to store the currently executing address
        private ushort PC = 0x200;

        //The stack pointer (SP) is 8-bit, it is used to point to the topmost level of the stack.
        private byte SP;

        private ushort[] Stack = new ushort[16];

        private Random random = new Random();

        public Display Display = new Display();

        public Memory Memory = new Memory();
         

        // The computers which originally used the Chip-8 Language had a 16-key hexadecimal keypad
        public bool[] KeyState { get; set; } = new bool[16] ;

        private bool DisplayGet(int i)
        {
            return Display.PixelsState[i];
        }

        public CPU()
        {
            //opcodes mapping

            MainOpcodeMap = new Dictionary<ushort, Action<Opcode>>
            {
                {0x0000, StartLookup },
                {0x1000, JP },
                {0x2000, CALL },
                {0x3000, SE_Vx },
                {0x4000, SNE_Vx },
                {0x5000, SE_Vx_Vy },
                {0x6000, LD_Vx },
                {0x7000, ADD_Vx },
                {0x8000, ArithmeticsOpcodeMapLookup },
                {0x9000, SNE_Vx_Vy },
                {0xA000, LD_I },
                {0xB000, JP_V0 },
                {0xC000, RND_Vx },
                {0xD000, DRW_Vx_Vy},
                {0xE000, KeyboardLookup },
                {0xF000, LoadsOpcodeMapLookup },
            };

            ArithmeticsOpcodeMap = new Dictionary<ushort, Action<Opcode>>
            {
                {0x00, LD_Vx_Vy },
                {0x01, OR_Vx_Vy },
                {0x02, AND_Vx_Vy },
                {0x03, XOR_Vx_Vy },
                {0x04, ADD_Vx_Vy },
                {0x05, SUB_Vx_Vy },
                {0x06, SHR_Vx_Vy },
                {0x07, SUBN_Vx_Vy },
                {0x0E, SHL_Vx_Vy },
            };

            LoadsOpcodeMap = new Dictionary<ushort, Action<Opcode>>
            {
                {0x07, LD_Vx_DT },
                {0x0A, LD_Vx_K },
                {0x15, LD_DT_Vx },
                {0x18, LD_ST_Vx },
                {0x1E, ADD_I_Vx },
                {0x29, LD_F_Vx },
                {0x33, LD_B_Vx },
                {0x55, LD_I_Vx },
                {0x65, LD_Vx_I },
            };

        }

        public void Reset()
        {
            Display.ClearDisplay();
            Memory = new Memory();
            Stack = new ushort[16];
            V = new byte[16];
            DelayTimer = 0;
            SoundTimer = 0;
            PC = 0x200;
            SP = 0;
            I = 0;
        }



        /// <summary>
        /// A full cycle of CPU including :
        /// fetching, decoding and executing opcode. 
        /// </summary>
        /// 

        public void FullCycle()
        {

            byte[] codedOpcode = FetchOpcode();
            ushort decodedOpcode = DecodeOpcode(codedOpcode);
            Opcode opcode = new Opcode(decodedOpcode);

            if (MainOpcodeMap.ContainsKey(opcode.FirstNibble))
            {
                MainOpcodeMap[(opcode.FirstNibble)](opcode);
            }
            else
            {
                throw new Exception($"Uknown Opcode {opcode.FullCode}");
            }

            PC = (ushort)(PC + 2);

        }

        /// <summary>
        /// Decrementing timers should
        /// be done every 16.666 seconds
        /// </summary>
        public void DecrementeTimers()
        {
            if (SoundTimer == 1)
            beep.BeginInvoke((a) => { beep.EndInvoke(a); }, null);
            if (SoundTimer > 0)
                SoundTimer--;
            if (DelayTimer > 0)
                DelayTimer--;
        }

        private byte[] FetchOpcode()
        {
            byte[] output = new byte[2];

            output[0] = Memory.MemoryMap[PC];
            output[1] = Memory.MemoryMap[PC + 1];

            return output;
        }

        private ushort DecodeOpcode(byte[] codedOpcode)
        {
            ushort output = (ushort)(codedOpcode[0] << 8 | codedOpcode[1]);

            return output;
        }
        private void ArithmeticsOpcodeMapLookup(Opcode opcode)
        {
            if (ArithmeticsOpcodeMap.ContainsKey(opcode.N))
            {
                ArithmeticsOpcodeMap[opcode.N](opcode); 
            }
            else
                throw new Exception($"Uknown Opcode {opcode.FullCode}");

        }
        private void LoadsOpcodeMapLookup(Opcode opcode)
        {
            if (LoadsOpcodeMap.ContainsKey(opcode.KK))
            {
                LoadsOpcodeMap[opcode.KK](opcode); 
            }
            else
                throw new Exception($"Uknown Opcode {opcode.FullCode}");
        }

        private void KeyboardLookup(Opcode opcode)
        {
            if (opcode.KK == 0x9E)
                SKP_Vx(opcode);
            else if (opcode.KK == 0xA1)
                SKNP_Vx(opcode);
            else
                throw new Exception($"Uknown Opcode {opcode.FullCode}");
        }

        private void StartLookup(Opcode opcode)
        {
            if (opcode.KK == 0xE0)
                CLS(opcode);
            else if (opcode.KK == 0xEE)
                RET(opcode);
            else
                throw new Exception($"Uknown Opcode {opcode.FullCode}");
        }

        //Hacky soulution for sound
        Action beep = ConsoleBeep;
        private static void ConsoleBeep()
        {
            Console.Beep(500, 500);
        }

    }
}
