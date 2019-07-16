using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDLLayer.PixleParsers
{
    internal class FalloutPixleParser : IPixleParser
    {
        public uint  ParsePixels(bool status)
        {
            if (status)
            {
                return 0x003300FF;
            }
            else
            {
                return 0x9CA89CFF;
            }

        }
    }
}
