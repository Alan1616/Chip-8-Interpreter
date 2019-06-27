using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Architecture;
using System.Threading;
using SDL2;
using SDLLayer;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Chip_8_ConsoleDisplay
{
    class Program
    {
        static bool isRunning = false;
        static bool falloutModeFlag = false;
        static void Main(string[] args)
        {                     
            CPU c1 = new CPU();
            Dictionary <string, Action<CPU,string>> ConsoleCommandsMap;
            ConsoleCommandsMap = new Dictionary<string, Action<CPU,string>>
            {
                {"Help",DisplayHelpCommand },
                {"Run",StartEmulatorCommand },
                {"LoadRom",LoadRomCommand },
                {"SetClockRate",SetClockRateCommand },
                {"FalloutMode", SetFalloutModeCommand },
                {"SuperChipMode",SuperChipModeCommand },
                {"Quit",QuitCommand },
            };

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Welcome to my Chip-8 Emulator! type in Help for commands!");
                c1.Initialize();

                while (isRunning == false)
                {
                    string line = "";
                    Console.Write(">");
                    line = Console.ReadLine();
                    string command;
                    string value = "0";
                    if (line.Contains(" "))
                    {
                        command = line.Substring(0, line.IndexOf(' '));
                        value = line.Substring(line.IndexOf(' ') + 1);
                    }
                    else
                        command = line;
                    if (ConsoleCommandsMap.ContainsKey(command))
                    {
                        ConsoleCommandsMap[command](c1, value);
                    }
                    else
                    {
                        Console.WriteLine(">Unknown Command");
                    }
                }

                if (isRunning)
                {
                    SDLWindowDisplay s1 = new SDLWindowDisplay(c1, falloutModeFlag);
                    s1.TriesToQuitWhileWaitingEvent += SDLWindowDisplay_TriesToQuitWhileWaitingEvent;

                    while (isRunning)
                    {
                        c1.FullCycle();
                        s1.render();
                        s1.HandleEvents(ref isRunning);
                    }
                    Console.WriteLine("Exiting Emulator Window");
                    s1.TriesToQuitWhileWaitingEvent -= SDLWindowDisplay_TriesToQuitWhileWaitingEvent;
                    s1.Quit();
                }

            }

        }


        private static void SDLWindowDisplay_TriesToQuitWhileWaitingEvent(object sender, bool e)
        {
            isRunning = false;
        }

        private static ushort ConvertUInt16ToBigEndian(ushort value)
        {
            byte[] temp = BitConverter.GetBytes(value);
            Array.Reverse(temp);
            value = BitConverter.ToUInt16(temp, 0);
            return value;
        }

        private static void DisplayHelpCommand(CPU c1, string value)
        {
            Console.WriteLine("->Example of invoking command  >LoadRom TETRIS or >SetClockRate 200");
            Console.WriteLine("->Run - to run a program from specified ROM source");
            Console.WriteLine("->LoadRom \"roamtoload\" - specify ROM source");
            Console.WriteLine("->SetClockRate [target frequency in Hz] for example SetCPUFreq 500 sets CPU frequency to 500 HZ (range 200-2000), defualt is 600 ");
            Console.WriteLine("->FalloutMode [on/off] - Fallout mode on turns colors to green and gray while off is true to orginal Chip-8 mono, defualt is off");
            Console.WriteLine("->Quit - Bye!!");
            Console.WriteLine("->SuperChipMode [on/off] - Allows to run SuperChip-8 programs - not implemented yet so don't bother");
        }
        private static void StartEmulatorCommand(CPU c1, string value)
        {
            if (c1.Memory.currentROMPath != null)
            {
                isRunning = true;
                Console.Clear();
                Console.WriteLine($">Runing game from specified {c1.Memory.currentROMPath} file ROM with CPUFrequency = {c1.CPUClockRate} Hz enjoy!!!!");
            }
            else
            {
                Console.WriteLine(">You propably should load your cartridge first ^-^");
            }
        }
        private static void LoadRomCommand(CPU c1, string value)
        {
            if (File.Exists(value))
            {
                c1.Memory.LoadProgram($@"{value}");
                Console.WriteLine($">Program {c1.Memory.currentROMPath} loaded!");
            }
            else
            {
                Console.WriteLine(">Unable to load specifed file check if file exists");
            }
        }
        private static void SetClockRateCommand(CPU c1, string value)
        {
            int clock = 0;
            bool isGood = false;
            isGood = int.TryParse(value, out clock);
            if (isGood && clock >= 200 && clock <= 2000)
            {
                c1.CPUClockRate = int.Parse(value);
                Console.WriteLine($">Current cpu frequency = {c1.CPUClockRate} Hz");
            }
            else
            {
                Console.WriteLine(">Specified value is out of range");
            }
        }
        private static void SetFalloutModeCommand(CPU c1, string value)
        {
            if (value == "on")
            {
                falloutModeFlag = true;
                Console.WriteLine(">FalloutMode enabled");
            }
            else if (value == "off")
            {
                falloutModeFlag = false;
                Console.WriteLine(">FalloutMode disabled");
            }
            else
            {
                Console.WriteLine(">Wrong value choose on or off instead");
            }
        }
        private static void SuperChipModeCommand(CPU c1, string value)
        {
            Console.WriteLine(">Told you not to bother!!!!");
        }
        private static void QuitCommand(CPU c1, string value)
        {
            Environment.Exit(0);
        }

    }
}
