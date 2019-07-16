using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDLLayer.PixleParsers
{
    internal interface IPixleParser
    {
        uint ParsePixels(bool status);
    }
}
