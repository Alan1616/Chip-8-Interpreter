using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Architecture
{
    public class Display : IDirectDisplayAccess
    {
        public bool[] PixelsState { get; } = new bool[64 * 32];

        public void ClearDisplay()
        {
            for (int i = 0; i < PixelsState.Length; i++)
            {
                PixelsState[i] = false;
            }
        }

        public bool XORPixel(ushort cordinates)
        {
            bool flag = false;

            cordinates = (ushort)(cordinates % 2048);

            if (PixelsState[cordinates] == true)
            {
                PixelsState[cordinates] = false;
                flag = true;
            }
            else
                PixelsState[cordinates] = true;
            return flag;
        }

        [Obsolete("Use graphic Library window instead!")]
        public void DrawDisplay()
        {
            Console.SetCursorPosition(0, 0);
            for (int i = 0; i < 32; i++)
            {
                for (int j = 0; j < 64; j++)
                {
                    if (PixelsState[j + i *64 ])
                    {
                        Console.Write("#");

                    }
                    else
                    {
                        Console.Write(" ");
                    }
                }

                Console.WriteLine();
            }
        }

    }
}
