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

    }
}
