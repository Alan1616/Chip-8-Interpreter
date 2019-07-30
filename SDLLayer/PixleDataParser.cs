using System;
using System.Collections.Generic;

namespace DisplayLibrary
{
    internal  static class PixleDataParser
    {
        internal static readonly Dictionary<DisplayMode, Func<bool, uint>> pixleParserMethodsMap = new Dictionary<DisplayMode, Func<bool, uint>>
        {
            { DisplayMode.DefaultMode, ParseDefault},
            { DisplayMode.FalloutMode, ParseFallout},
            { DisplayMode.RedMode, ParseRed},
            { DisplayMode.BlueMode, ParseBlue},
        };

        private static uint ParseBlue(bool status)
        {
            if (status)
                return 0x0000FFFF;

            return 0xADD8E6FF;
        }

        private static uint ParseRed(bool status)
        {
            if (status)
                return 0xDC143CFF;

             return 0x00000000;

        }

        private static uint ParseFallout(bool status)
        {
            if (status)
                return 0x003300FF;

            return 0x9CA89CFF;
        }

        private static uint ParseDefault(bool status)
        {
            if (status)
                return 0xFFFFFFFF;

            return 0x00000000;

        }


    }
}
