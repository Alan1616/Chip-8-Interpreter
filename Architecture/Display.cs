using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Architecture
{
    public class Display
    {
        public bool[] Pixels = new bool[64 * 32];


        public void ClearDisplay()
        {
            for (int i = 0; i < Pixels.Length; i++)
            {
                Pixels[i] = false;
            }
        }

        public bool XORPixel(ushort cordinates)
        {
            bool flag = false;

            cordinates = (ushort)(cordinates % 2048);

            if (Pixels[cordinates] == true)
            {
                Pixels[cordinates] = false;
                flag = true;
            }
            else
                Pixels[cordinates] = true;
            return flag;
        }

        public void DrawDisplay()
        {
            //Console.Clear();
            Console.SetCursorPosition(0, 0);
            string line = "";
            for (int i = 0; i < 32; i++)
            {
                for (int j = 0; j < 64; j++)
                {
                    if (Pixels[j + i *64 ])
                    {
                        //Console.SetCursorPosition(j, i);
                        Console.Write("#");
                        //line += "#";

                    }
                    else
                    {
                        //Console.SetCursorPosition(j, i);
                        Console.Write(" ");
                        //line += " ";
                    }
                }

                Console.WriteLine(line);

                //Thread.Sleep(16);

            }
        }


    }
}
