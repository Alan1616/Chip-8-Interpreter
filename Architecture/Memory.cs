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
        public string currentROMPath { get; set; }

        public Memory()
        {
            SetUpFont(0x000);
        }
        private void SetUpFont(int startLocation)
        {
            try
            {
                if (!File.Exists("BuildInFontFile.txt"))
                {
                    CreateDefaultFontFile();
                }

                string[] fonts = LoadFontFile();
                LoadFont(startLocation,fonts);
            }
            catch (Exception)
            {
                //TODO log it somwhere.
                throw;
            }
       
        }

        private string[] LoadFontFile()
        {
            string[] lines = File.ReadAllLines("BuildInFontFile.txt");
            return lines;
        }

        private void LoadFont(int startLocation, string[] fontFile)
        {
            int stopLocation = fontFile.Length + 80;
            if (stopLocation >= 512)
                throw new OutOfMemoryException();
            for (int i = 0; i < fontFile.Length; i++)
            {
                MemoryMap[i + startLocation] = Convert.ToByte(fontFile[i], 16);
            }
        }

        private void CreateDefaultFontFile()
        {
            string[] defaultFont = new string[]
            {
            "0xF0", "0x90", "0x90", "0x90", "0xF0", "0x20", "0x60", "0x20",
            "0x20", "0x70", "0xF0", "0x10", "0xF0", "0x80", "0xF0", "0xF0",
            "0x10", "0xF0", "0x10", "0xF0", "0x90", "0x90", "0xF0", "0x10",
            "0x10", "0xF0", "0x80", "0xF0", "0x10", "0xF0", "0xF0", "0x80",
            "0xF0", "0x90", "0xF0", "0xF0", "0x10", "0x20", "0x40", "0x40",
            "0xF0", "0x90", "0xF0", "0x90", "0xF0", "0xF0", "0x90", "0xF0",
            "0x10", "0xF0", "0xF0", "0x90", "0xF0", "0x90", "0x90", "0xE0",
            "0x90", "0xE0", "0x90", "0xE0", "0xF0", "0x80", "0x80", "0x80",
            "0xF0", "0xE0", "0x90", "0x90", "0x90", "0xE0", "0xF0", "0x80",
            "0xF0", "0x80", "0xF0", "0xF0", "0x80", "0xF0", "0x80", "0x80"
            };

            using (StreamWriter s = new StreamWriter("BuildInFontFile.txt"))
            {
                foreach (string singleLine in defaultFont)
                {
                    s.Write(singleLine.ToString() + Environment.NewLine);
                }
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
