using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Architecture
{
    public class CPU
    {

        private byte nextKeyPressed;

        //General purpose 8-bit registers, referred to as Vx, where x is a hexadecimal digit (0 through F)
        public byte[] V = new byte[16];

        //16-bit register called I. This register is generally used to store memory addresses, so only the lowest (rightmost) 12 bits are usually used.
        public ushort I;

        /*Chip-8 also has two special purpose 8-bit registers, for the delay and sound timers. 
          When these registers are non-zero, they are automatically decremented at a rate of 60Hz.*/
        public byte DelayTimer;
        public byte SoundTimer;

        //The program counter(PC) is 16-bit, and is used to store the currently executing address
        public ushort PC = 0x200;

        //The stack pointer (SP) is 8-bit, it is used to point to the topmost level of the stack.
        public byte SP;

        public ushort[] Stack = new ushort[16];

        public Display Display = new Display();

        private Random random = new Random();

        public Memory m1 = new Memory();

        public void Initialize()
        {
            Display.ClearDisplay();
            m1 = new Memory();
            Stack = new ushort[16];
            V = new byte[16];
            DelayTimer = 0;
            SoundTimer = 0;
            PC = 0x200;
            SP = 0;
            I = 0;
        }

        private delegate void DisplayDrawerDelegate();

        public void ExecuteOpcode(ushort opcode)
        {
            ushort nibble = (ushort)(opcode & 0xF000);
            byte x = (byte)((opcode & 0x0F00) >> 8);
            byte y = (byte)((opcode & 0x00F0) >> 4);
            byte kk = (byte)((opcode & 0x00FF));
            byte n = (byte)((opcode & 0x000F) );
            ushort nnn = (ushort)((opcode & 0x0FFF));
            DisplayDrawerDelegate d1 = new DisplayDrawerDelegate(Display.DrawDisplay);
            //Console.WriteLine($"{nibble:X}");

            switch (nibble)
            {
                case 0x0000:
                    switch (n)
                    {
                        case 0x00:
                            CLS();
                            break;
                        case 0x0E:
                            RET();
                            break;
                        default:
                            throw new Exception($"Unknown opcode = {opcode}");
                    }
                    break;
                case 0x1000:
                    JP(nnn);
                    break;
                case 0x2000:
                    CALL(nnn);
                    break;
                case 0x3000:
                    SE_Vx(x, kk);
                    break;
                case 0x4000:
                    SNE_Vx(x, kk);
                    break;
                case 0x5000:
                    if (n==0)
                    {
                        SE_Vx_Vy(x, y);
                        break;
                    }
                    else
                    {
                        throw new Exception($"Unknown opcode = {opcode}");
                    }
                case 0x6000:
                    LD_Vx(x, kk);
                    break;
                case 0x7000:
                    ADD_Vx(x, kk);
                    break;
                case 0x8000:
                    switch (n)
                    {
                        case 0:
                            LD_Vx_Vy(x, y);
                            break;
                        case 1:
                            OR_Vx_Vy(x, y);
                            break;
                        case 2:
                            AND_Vx_Vy(x, y);
                            break;
                        case 3:
                            XOR_Vx_Vy(x, y);
                            break;
                        case 4:
                            ADD_Vx_Vy(x,y);
                            break;
                        case 5:
                            SUB_Vx_Vy(x, y);
                            break;
                        case 6:
                            SHR_Vx_Vy(x, y);
                            break;
                        case 7:
                            SUBN_Vx_Vy(x, y);
                            break;
                        case 14:
                            SHL_Vx_Vy(x, y);
                            break;
                    }
                    break;
                case 0x9000:
                    if (n == 0)
                    {
                        SNE_Vx_Vy(x, y);
                        break;
                    }
                    else
                    {
                        throw new Exception($"Unknown opcode = {opcode}");
                    }
                case 0xA000:
                    LD_I(nnn);
                    break;
                case 0xB000:
                    JP_V0(nnn);
                    break;
                case 0xC000:
                    RND_Vx(x, kk, random);
                    break;
                case 0xD000:
                    DRW_Vx_Vy(x, y, n);
                    d1();
                    break;
                case 0xE000:
                    switch (kk)
                    {
                        case 0x9E:
                            SKP_Vx(x);
                            break;
                        case 0xA1:
                            SKNP_Vx(x);
                            break;
                        default:
                            throw new Exception($"Unknown opcode = {opcode}");
                    }
                    break;
                case 0xF000:
                    switch (kk)
                    {
                        case 0x07:
                            LD_Vx_DT(x);
                            break;
                        case 0x0A:
                            LD_Vx_K(x);
                            break;
                        case 0x15:
                            LD_DT_Vx(x);
                            break;
                        case 0x18:
                            LD_ST_Vx(x);
                            break;
                        case 0x1E:
                            ADD_I_Vx(x);
                            break;
                        case 0x29:
                            LD_F_Vx(x);
                            break;
                        case 0x33:
                            LD_B_Vx(x);
                            break;
                        case 0x55:
                            LD_I_Vx(x);
                            break;
                        case 0x65:
                            LD_Vx_I(x);
                            break;
                        default:
                            throw new Exception($"Unknown opcode = {opcode}");
                    }
                    break;

                default:
                    throw new Exception($"Nibble out of bound");
            }
        }

        private Stopwatch watch = new Stopwatch();

        public ushort FullCycle()
        {
            if (!watch.IsRunning)
                watch.Start();
            if (watch.ElapsedMilliseconds > 16)
            {
                DecrementeTimers();
                watch.Reset();
            }


            byte[] codedOpcode = FetchOpcode();
            ushort decodedOpcode = DecodeOpcode(codedOpcode);
            ExecuteOpcode(decodedOpcode);
            PC = (ushort)(PC + 2);
            //if (SoundTimer > 0)
            //    SoundTimer--;
            //if (DelayTimer > 0)
            //    DelayTimer--;
            //DecrementeTimers();


            return decodedOpcode;
        }


        public void DecrementeTimers()
        {
                if (SoundTimer > 0)
                    SoundTimer--;
                if (DelayTimer > 0)
                    DelayTimer--;
        }


        private byte[] FetchOpcode()
        {
            byte[] output = new byte[2];

            output[0] = m1.MemoryMap[PC];
            output[1] = m1.MemoryMap[PC+1];

            return output;
        }

        private ushort DecodeOpcode(byte[] codedOpcode)
        {
            ushort output = (ushort)(codedOpcode[0] << 8 | codedOpcode[1]);

            return output;
        }

        //NameOfInstruction[Number of instruction] - short description
        //CLS[1] - Clear the display.
        private void CLS()
        {
            Display.ClearDisplay();
        }//1T
        //RET[2] - Sets the program counter to the address at the top of the stack, 
        //then subtracts 1 from the stack pointer.
        private void RET()
        {
            PC = Stack[SP];
            SP--;
        }//2T
        //JP[3] - Sets the program counter to nnn.
        private void JP(ushort nnn)
        {
            PC = (ushort)(nnn-2);
            //PC = nnn;
        }//3

        private void CALL(ushort nnn)
        {
            SP++;
            Stack[SP] = PC;
            PC = (ushort)(nnn - 2);
            //PC = nnn;
        }//4T

        private void SE_Vx(byte x, byte kk)
        {
            if (V[x] == kk)
            {
                PC = (ushort)(PC + 2);
            }
        }//5

        private void SNE_Vx(byte x, byte kk)
        {
            if (V[x] != kk)
            {
                PC = (ushort)(PC + 2);
            }
        }//6

        private void SE_Vx_Vy(byte x, byte y)
        {
            if (V[x] == V[y])
            {
                PC = (ushort)(PC + 2);
            }
        }//7

        private void LD_Vx(byte x, byte kk)
        {
            V[x] = kk;
        }//8

        private void ADD_Vx(byte x, byte kk)
        {
            V[x] = (byte)(V[x] + kk);
        }//9

        private void LD_Vx_Vy(byte x, byte y)
        {
            V[x] = V[y];
        }//10

        private void OR_Vx_Vy(byte x, byte y)
        {
            V[x] =(byte) (V[x] | V[y]);
        }//11

        private void AND_Vx_Vy(byte x, byte y)
        {
            V[x] = (byte)(V[x] & V[y]);
        }//12

        private void XOR_Vx_Vy(byte x, byte y)
        {
            V[x] = (byte)(V[x] ^ V[y]);
        }//13

        private void ADD_Vx_Vy(byte x, byte y)
        {
            ushort sum = (ushort)(V[x] + V[y]);
            if (sum > 255)
            {
                V[15] = 1;
            }
            else
            {
                V[15] = 0;
            }
            V[x] = (byte)(sum & 0x00FF);

        }//14

        private void SUB_Vx_Vy(byte x, byte y)//15
        {
            if (V[x] > V[y])
            {
                V[15] = 1;
            }
            else
            {
                V[15] = 0;
            }
            V[x] = (byte)((V[x] - V[y]) & 0x00FF);

        }

        private void SHR_Vx_Vy(byte x, byte y)
        {
            if (V[x]%2==1)
            {
                V[15] = 1;
            }
            else
            {
                V[15] = 0;
            }
            V[x] = (byte)(V[x] / 2);
        }//16

        private void SUBN_Vx_Vy(byte x, byte y)
        {
            if (V[y] > V[x])
            {
                V[15] = 1;
            }
            else
            {
                V[15] = 0;
            }
            V[x] = (byte)((V[y] - V[x]) & 0x00FF);
        }//17

        private void SHL_Vx_Vy(byte x, byte y)
        {
            if ((byte)(V[x] & 0x80)==0x80)
            {
                V[15] = 1;
            }
            else
            {
                V[15] = 0;
            }
            V[x] = (byte)(V[x] * 2);
        }//18

        private void SNE_Vx_Vy(byte x, byte y)
        {
            if (V[x]!=V[y])
            {
                PC = (ushort)(PC + 2);
            }
        }//19

        private void LD_I(ushort nnn)
        {
            I = nnn;
        }//20

        private void JP_V0(ushort nnn)
        {
            PC = (ushort)(nnn + V[0]);
        }//21

        private void RND_Vx(byte x, byte kk, Random random)
        {
            byte rand = (byte)(random.Next(0,256));
            V[x] = (byte)(rand & kk);
        }//22

        private void DRW_Vx_Vy(byte x, byte y, byte n)
        {
            V[15] = 0;
            byte[] pixels = new byte[n];
            byte xCoordinate = V[x];
            byte yCoordinate = V[y];

            for (int i = 0; i < n; i++)
            {
                pixels[i] = m1.MemoryMap[I + i];
            }

            for (int i = 0; i < pixels.Length; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if ((pixels[i] & (0x80 >> j)) != 0)
                    {
                        bool flag = Display.XORPixel((ushort)((xCoordinate + j + (yCoordinate + i) * 64)));
                        if (flag)
                        {
                            V[15] = 1;
                        }
                    }
                }
            }



        }//23 

        private void SKP_Vx(byte x)
        {
            if (nextKeyPressed == V[x])
                PC = (ushort)(PC + 2);
            //throw new NotImplementedException();
        }//24 not fully implemented

        private void SKNP_Vx(byte x)
        {
            if (nextKeyPressed != V[x])
                PC = (ushort)(PC + 2);
            // throw new NotImplementedException();
        }//25 not fully implemented

        private void LD_Vx_DT(byte x)
        {
            V[x] = DelayTimer;
        }//26

        private void LD_Vx_K(byte x)
        {
            V[x] = Byte.Parse(Console.ReadLine());
            //V[x] = 1;
        } //27 simulates waiting for keypress - not fully implemented

        private void LD_DT_Vx(byte x)
        {
            DelayTimer = V[x];
        }//28

        private void LD_ST_Vx(byte x)
        {
            SoundTimer = V[x];
        }//29

        private void ADD_I_Vx(byte x)
        {
            I = (ushort)(I + V[x]);
        }//30

        private void LD_F_Vx(byte x)
        {
            I = (ushort)(V[x] * 5);
        }//31 

        private void LD_B_Vx(byte x)
        {
            byte hundreds = (byte) ((V[x] % 1000 - V[x]%100)/100);
            byte tens = (byte)((V[x] % 100 - V[x]%10)/10);
            byte ones = (byte)(V[x] % 10);

            m1.MemoryMap[I] = hundreds;
            m1.MemoryMap[I + 1] = tens;
            m1.MemoryMap[I + 2] = ones;
        }//32

        private void LD_I_Vx(byte x)
        {
            for (int i = 0; i <= x; i++)
            {
                m1.MemoryMap[I + i] = V[i];
            }
        }//33

        private void LD_Vx_I(byte x)
        {
            for (int i = 0; i <= x; i++)
            {
                V[i] = m1.MemoryMap[I + i]; 
            }
        }//34 

        private void SYS(ushort nnn)
        {
            throw new NotImplementedException();
        }//0 not imp - propably not nescesey


    }
}
