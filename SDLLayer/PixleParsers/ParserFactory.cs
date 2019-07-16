
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDLLayer.PixleParsers
{
    internal static class ParserFactory
    {
        internal static IPixleParser GetPixleParser(DisplayMode colorScheme)
        {
            switch (colorScheme)
            {
                case DisplayMode.FalloutMode:
                    return new FalloutPixleParser();
                case DisplayMode.RedMode:
                    return new RedPixleParser();
                case DisplayMode.BlueMode:
                    return new BluePixleParser();
                default:
                    return new DefualtPixleParser();

            }

        }
    }
}

