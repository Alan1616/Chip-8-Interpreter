using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDLLayer.PixleParsers
{
    class BluePixleParser: IPixleParser
    {
        public uint ParsePixels(bool status)
        {
            if (status)
            {
                return 0x0000FFFF;
            }
            else
            {
                return 0xADD8E6FF;
            }
        }
    }
}
