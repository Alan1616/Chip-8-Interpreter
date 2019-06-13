using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            if (cordinates >= 2048)
                return flag;

            if (Pixels[cordinates] == true)
            {
                Pixels[cordinates] = false;
                flag = true;
            }
            else
                Pixels[cordinates] = true;
            return flag;
        }


    }
}
