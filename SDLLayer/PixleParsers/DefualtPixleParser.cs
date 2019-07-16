using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDLLayer.PixleParsers
{
    internal class DefualtPixleParser : IPixleParser
    {
        public uint ParsePixels(bool status)
        {
            if (status)
            {
                 return 0xFFFFFFFF;
            }
            else
            {
                return 0x00000000;
            }
        }
    }
}
