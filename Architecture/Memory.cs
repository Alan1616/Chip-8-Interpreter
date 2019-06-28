using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Architecture
{
    public class Memory
    {
        public byte[] MemoryMap = new byte[4096];
        public string currentROMPath;

        public Memory()
        {
            LoadFont(0x000);
        }
        private void LoadFont(int startLocation)
        {
            try
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
            catch (FileNotFoundException)
            {
                File.Create("BuildInFontFile.txt");
            }
            catch (Exception exception)
            {
                throw exception;
            }
       
        }

        public void LoadProgram(string location)
        {
            ClearProgramMemory();
            currentROMPath = location;
            BinaryReader b1 = new BinaryReader(File.Open(location, FileMode.Open), System.Text.Encoding.BigEndianUnicode);
            int i = 0;
            while (b1.BaseStream.Position < b1.BaseStream.Length)
            {
                if (0x200 + i > 0xFFF)
                    throw new OutOfMemoryException();
                byte oneByte = b1.ReadByte();
                MemoryMap[0x200 + i] = oneByte;
                i++;
            }
            b1.Close();      
        }

        private void ClearProgramMemory()
        {
            for (int i = 0x200; i < MemoryMap.Length; i++)
            {
                MemoryMap[i] = 0;
            }
        }
    }
}
