using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Architecture
{
    internal class Memory
    {
        public byte[] MemoryMap = new byte[4096];


        internal Memory()
        {
            LoadFont(0x000);
        }

        private void LoadFont(int startLocation)
        {
            string[] lines = File.ReadAllLines("BuildInFontFile.txt");
            int stopLocation = lines.Length + 80;
            if (stopLocation >= 512)
                throw new OutOfMemoryException();
            for (int i = 0; i < lines.Length; i++)
            {
                MemoryMap[startLocation] = Convert.ToByte(lines[i], 16);
                startLocation++;
            }
        }
        
    }
}
