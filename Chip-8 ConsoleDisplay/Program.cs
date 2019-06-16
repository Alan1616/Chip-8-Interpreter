using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Architecture;
using System.Threading;

namespace Chip_8_ConsoleDisplay
{
    class Program
    {
        static void Main(string[] args)
        {
            CPU c1 = new CPU();
            c1.m1.LoadProgram(@"heart_monitor.ch8");


            while (true)
            {
                //Console.WriteLine($"{ c1.FullCycle():X4}");
                c1.FullCycle();
            }

            //BinaryReader b1 = new BinaryReader(File.Open("Chip8 Picture.ch8", FileMode.Open), System.Text.Encoding.BigEndianUnicode);
            //while (b1.BaseStream.Position < b1.BaseStream.Length)
            //{
            //    ushort opcode = ConvertUInt16ToBigEndian((b1.ReadUInt16()));
            //    Console.WriteLine($"{opcode:X4}");

            //    //c1.ExecuteOpcode(opcode);
            //}

            //b1.Close();


            Console.ReadKey();
        }

        private static ushort ConvertUInt16ToBigEndian(ushort value)
        {
            byte[] temp = BitConverter.GetBytes(value);
            Array.Reverse(temp);
            value = BitConverter.ToUInt16(temp, 0);
            return value;
        }

    }
}
