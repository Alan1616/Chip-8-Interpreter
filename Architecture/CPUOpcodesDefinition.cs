using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Architecture
{
    public partial class CPU
    {
        //NameOfInstruction[Number of instruction] - short description
        //CLS[1] - Clear the display.
        private void CLS(Opcode opcode)
        {
            Display.ClearDisplay();
        }//1T
        //RET[2] - Sets the program counter to the address at the top of the stack, 
        //then subtracts 1 from the stack pointer.
        private void RET(Opcode opcode)
        {
            PC = Stack[SP];
            SP--;
        }//2T
        //JP[3] - Sets the program counter to nnn.
        private void JP(Opcode opcode)
        {
            PC = (ushort)(opcode.NNN - 2);
        }//3

        private void CALL(Opcode opcode)
        {
            SP++;
            Stack[SP] = PC;
            PC = (ushort)(opcode.NNN - 2);
        }//4T

        private void SE_Vx(Opcode opcode)
        {
            if (V[opcode.X] == opcode.KK)
            {
                PC = (ushort)(PC + 2);
            }
        }//5

        private void SNE_Vx(Opcode opcode)
        {
            if (V[opcode.X] != opcode.KK)
            {
                PC = (ushort)(PC + 2);
            }
        }//6

        private void SE_Vx_Vy(Opcode opcode)
        {
            if (V[opcode.X] == V[opcode.Y])
            {
                PC = (ushort)(PC + 2);
            }
        }//7

        private void LD_Vx(Opcode opcode)
        {
            V[opcode.X] = opcode.KK;
        }//8

        private void ADD_Vx(Opcode opcode)
        {
            V[opcode.X] = (byte)(V[opcode.X] + opcode.KK);
        }//9

        private void LD_Vx_Vy(Opcode opcode)
        {
            V[opcode.X] = V[opcode.Y];
        }//10

        private void OR_Vx_Vy(Opcode opcode)
        {
            V[opcode.X] = (byte)(V[opcode.X] | V[opcode.Y]);
        }//11

        private void AND_Vx_Vy(Opcode opcode)
        {
            V[opcode.X] = (byte)(V[opcode.X] & V[opcode.Y]);
        }//12

        private void XOR_Vx_Vy(Opcode opcode)
        {
            V[opcode.X] = (byte)(V[opcode.X] ^ V[opcode.Y]);
        }//13

        private void ADD_Vx_Vy(Opcode opcode)
        {
            ushort sum = (ushort)(V[opcode.X] + V[opcode.Y]);
            if (sum > 255)
            {
                V[0xF] = 1;
            }
            else
            {
                V[0xF] = 0;
            }
            V[opcode.X] = (byte)(sum & 0x00FF);

        }//14

        private void SUB_Vx_Vy(Opcode opcode)//15
        {
            if (V[opcode.X] > V[opcode.Y])
            {
                V[0xF] = 1;
            }
            else
            {
                V[0xF] = 0;
            }
            V[opcode.X] = (byte)((V[opcode.X] - V[opcode.Y]) & 0x00FF);

        }

        private void SHR_Vx_Vy(Opcode opcode)
        {
            if (V[opcode.X] % 2 == 1)
            {
                V[0xF] = 1;
            }
            else
            {
                V[0xF] = 0;
            }
            V[opcode.X] = (byte)(V[opcode.X] / 2);
        }//16

        private void SUBN_Vx_Vy(Opcode opcode)
        {
            if (V[opcode.Y] > V[opcode.X])
            {
                V[0xF] = 1;
            }
            else
            {
                V[0xF] = 0;
            }
            V[opcode.X] = (byte)((V[opcode.Y] - V[opcode.X]) & 0x00FF);
        }//17

        private void SHL_Vx_Vy(Opcode opcode)
        {
            if ((byte)(V[opcode.X] & 0x80) == 0x80)
            {
                V[0xF] = 1;
            }
            else
            {
                V[0xF] = 0;
            }
            V[opcode.X] = (byte)(V[opcode.X] * 2);
        }//18

        private void SNE_Vx_Vy(Opcode opcode)
        {
            if (V[opcode.X] != V[opcode.Y])
            {
                PC = (ushort)(PC + 2);
            }
        }//19
 
        private void LD_I(Opcode opcode)
        {
            I = opcode.NNN;
        }//20

        private void JP_V0(Opcode opcode)
        {
            PC = (ushort)(opcode.NNN + V[0]);
        }//21

        private void RND_Vx(Opcode opcode)
        {
            byte rand = (byte)(random.Next(0, 256));
            V[opcode.X] = (byte)(rand & opcode.KK);
        }//22

        private void DRW_Vx_Vy(Opcode opcode)
        {
            V[0xF] = 0;
            byte[] pixels = new byte[opcode.N];
            byte xCoordinate = V[opcode.X];
            byte yCoordinate = V[opcode.Y];

            for (int i = 0; i < opcode.N; i++)
            {
                pixels[i] = Memory.MemoryMap[I + i];
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
                            V[0xF] = 1;
                        }
                    }
                }
            }

        }//23 

        private void SKP_Vx(Opcode opcode)
        {
            if (keyState[V[opcode.X]] == true)
            {
                PC = (ushort)(PC + 2);
            }
        }//24 

        private void SKNP_Vx(Opcode opcode)
        {
            if (keyState[V[opcode.X]]==false)
            {
                PC = (ushort)(PC + 2);
            }           
        }//25

        private void LD_Vx_DT(Opcode opcode)
        {
            V[opcode.X] = DelayTimer;
        }//26

        private void LD_Vx_K(Opcode opcode)
        {
            AwaitsForKeypress = true;
            WaitForKeypressEvent?.Invoke(this, AwaitsForKeypress);

            for (byte i = 0; i < keyState.Length; i++)
            {
                if (keyState[i])
                {
                    V[opcode.X] = i;
                    keyState[i] = false;
                    break;
                }
            }

            //V[x] = 1;
        } //27 

        private void LD_DT_Vx(Opcode opcode)
        {
            DelayTimer = V[opcode.X];
        }//28

        private void LD_ST_Vx(Opcode opcode)
        {
            SoundTimer = V[opcode.X];
        }//29

        private void ADD_I_Vx(Opcode opcode)
        {
            I = (ushort)(I + V[opcode.X]);
        }//30

        private void LD_F_Vx(Opcode opcode)
        {
            I = (ushort)(V[opcode.X] * 5);
        }//31 

        private void LD_B_Vx(Opcode opcode)
        {
            byte hundreds = (byte)((V[opcode.X] % 1000 - V[opcode.X] % 100) / 100);
            byte tens = (byte)((V[opcode.X] % 100 - V[opcode.X] % 10) / 10);
            byte ones = (byte)(V[opcode.X] % 10);

            Memory.MemoryMap[I] = hundreds;
            Memory.MemoryMap[I + 1] = tens;
            Memory.MemoryMap[I + 2] = ones;
        }//32

        private void LD_I_Vx(Opcode opcode)
        {
            for (int i = 0; i <= opcode.X; i++)
            {
                Memory.MemoryMap[I + i] = V[i];
            }
        }//33

        private void LD_Vx_I(Opcode opcode)
        {
            for (int i = 0; i <= opcode.X; i++)
            {
                V[i] = Memory.MemoryMap[I + i];
            }
        }//34 

    }
}
