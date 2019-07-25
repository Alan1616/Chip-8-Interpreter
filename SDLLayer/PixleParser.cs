using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDLLayer
{
    internal  static class PixleParser
    {
        internal static readonly Dictionary<DisplayMode, Func<bool, uint>> pixleParserMethodsMap = new Dictionary<DisplayMode, Func<bool, uint>>
        {
            { DisplayMode.DefaultMode, ParseDefault},
            { DisplayMode.FalloutMode, ParseFallout},
            { DisplayMode.RedMode, ParseRed},
            { DisplayMode.BlueMode, ParseBlue},
        };

        internal static uint ParseBlue(bool status)
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

        internal static uint ParseRed(bool status)
        {
            if (status)
            {
                return 0xDC143CFF;
            }
            else
            {
                return 0x00000000;
            }
        }

        internal static uint ParseFallout(bool status)
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

        internal static uint ParseDefault(bool status)
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
