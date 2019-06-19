using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Architecture;
using System.Threading;
using SDL2;
using System.Runtime.InteropServices;

namespace Chip_8_ConsoleDisplay
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to my Chip-8 Emulator! type in Help for comands!");
            bool isRunning = false;
            CPU c1 = new CPU();
            while (isRunning == false)
            {
                string line = "";
                line = Console.ReadLine();
                string command;
                string value ="0";
                if (line.Contains(" "))
                {
                    command = line.Substring(0, line.IndexOf(' '));
                    value = line.Substring(line.IndexOf(' ')+1);
                }
                else
                    command = line;
                switch (command)
                {
                    case "Help":
                        Console.WriteLine("Run - to run a program from specified ROM source");
                        Console.WriteLine("LoadRom \"roamtoload\" - specify ROM source");
                        Console.WriteLine("SetCPUFreq [target freq in MHz] for example SetCPUFreq 500 sets CPU frequency to 500 MHZ (range 200-1200), defualt is 600 ");
                        Console.WriteLine("FalloutMode [on/off] - Fallout mode on turns colors to green and gray while off is true to orginal Chip-8 mono, defualt is off");
                        Console.WriteLine("Quit - Bye!!");
                        Console.WriteLine("SuperChipMode [on/off] - Allows to run SuperChip-8 programs - not implemented yet so don't bother");
                        break;
                    case "Run":
                        isRunning = true;
                        Console.Clear();
                        Console.WriteLine($"Runing game from specified {c1.m1.currentROMPath} file ROM with CPUFrequency = {c1.CPU_CLOCK} MHz enjoy!!!!");
                        break;
                    case "LoadRom":
                        c1.m1.LoadProgram($@"{value}");
                        break;
                    case "SetCPUFreq":
                        c1.CPU_CLOCK = int.Parse(value);
                        Console.WriteLine($"Current cpu frequency is = {value} MHz");
                        break;

                    default:
                        Console.WriteLine("Unknown Command");
                        break;
                }
                //Console.WriteLine(value);
            }


            
            if (isRunning)
            {
                SDLWindowDisplay s1 = new SDLWindowDisplay(c1);
                while (isRunning)
                {
                    //    //Console.WriteLine($"{ c1.FullCycle():X4}");
                    s1.HandleEvents(ref isRunning);
                    c1.FullCycle();
                    s1.render();
                    //Thread.Sleep(1);
                    //    //Thread.Sleep(1);
                    //    //Console.WriteLine($"V[6]={c1.V[6]}");
                    //    //Console.WriteLine($"V[7]={c1.V[7]}");
                }
                s1.Quit();
            }


            //do
            //{
            //    c1.FullCycle();
            //} while (!signaled);

           
    

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
//BinaryReader b1 = new BinaryReader(File.Open("Chip8 Picture.ch8", FileMode.Open), System.Text.Encoding.BigEndianUnicode);
//while (b1.BaseStream.Position < b1.BaseStream.Length)
//{
//    ushort opcode = ConvertUInt16ToBigEndian((b1.ReadUInt16()));
//    Console.WriteLine($"{opcode:X4}");

//    //c1.ExecuteOpcode(opcode);
//}

//b1.Close();