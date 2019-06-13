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
        public ushort PC;

        //The stack pointer (SP) is 8-bit, it is used to point to the topmost level of the stack.
        public byte SP;

        public ushort[] Stack = new ushort[16];

        public Display Display = new Display();

        public Random Random = new Random();

        internal Memory m1 = new Memory();

        public void ExecuteOpcode(ushort opcode)
        {
            ushort nibble = (ushort)(opcode & 0xF000);
            Console.WriteLine($"{nibble:X}");

            switch (nibble)
            {
                //CLS
                case 0x00E0:
                    Display.ClearDisplay();
                    break;
                //RET
                case 0x00EE:
                    PC = SP;
                    SP--;
                    break;



                default:
                    throw new Exception($"Unknown opcode = {opcode}");
            }
        }

        private void CLS()
        {
            Display.ClearDisplay();
        }//1
        private void RET()
        {
            PC = SP;
            SP--;
        }//2

        private void JP(ushort addr)
        {
            PC = addr;
        }//3

        private void CALL(ushort addr)
        {
            SP++;
            Stack[SP] = PC;
            PC = addr;
        }//4

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
            throw new NotImplementedException();
        }//23notimp

        private void SKP_Vx(byte x)
        {
            //if (nextKeyPressed == V[x])
            //    PC = (ushort)(PC + 2);
            throw new NotImplementedException();
        }//24notimp

        private void SKNP_Vx(byte x)
        {
            //if (nextKeyPressed != V[x])
            //    PC = (ushort)(PC + 2);
            throw new NotImplementedException();
        }//25notimp

        private void LD_Vx_DT(byte x)
        {
            V[x] = DelayTimer;
        }//26

        private void LD_Vx_K(byte x)
        {
            throw new NotImplementedException(); 
        } //27notimp

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
            throw new NotImplementedException();
        }//31not imp

        private void LD_B_Vx(byte x)
        {
            byte hundreds = (byte) ((V[x] % 1000 - V[x]%100)/100);
            byte tens = (byte)((V[x] % 100 - x%10)/10);
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
