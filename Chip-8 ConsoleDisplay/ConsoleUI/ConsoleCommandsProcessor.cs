using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Architecture;
using DisplayLibrary;

namespace Chip_8_ConsoleDisplay.ConsoleUI
{
    public static class ConsoleCommandsProcessor
    {
        static readonly Dictionary<string, Action<InterpreterInstance, string>> ConsoleCommandsMap = new Dictionary<string, Action<InterpreterInstance, string>>
        {
            {"help",DisplayHelpCommand },
            {"run",StartEmulatorCommand },
            {"loadrom",LoadRomCommand },
            {"setclockrate",SetClockRateCommand },
            {"displaymode", SetDisplayModeCommand },
            {"superchipmode",SuperChipModeCommand },
            {"quit",QuitCommand },
        };


        static readonly Dictionary<string, DisplayMode> DisplayModesMap = new Dictionary<string, DisplayMode>
        {
            {"fallout", DisplayMode.FalloutMode },
            {"default", DisplayMode.DefaultMode},
            {"blue", DisplayMode.BlueMode },
            { "red", DisplayMode.RedMode},
        };


        public static void GreetTheUser(this InterpreterInstance interpreter)
        {
            Console.Clear();
            Console.WriteLine("Welcome to my Chip-8 Emulator! type in Help for commands!");
        }

        public static void ProcessCommands(this InterpreterInstance interpreter)
        {
            string consoleInput;
            Console.Write(">");
            consoleInput = Console.ReadLine();

            string value = "0";
            string consoleCommand = "";

            if (consoleInput.Contains(" "))
            {
                consoleCommand = ParseToCommand(consoleInput).ToLower();
                value = ParseToValue(consoleInput);
            }
            else
            {
                consoleCommand = consoleInput.ToLower();
            }


            if (ConsoleCommandsMap.ContainsKey(consoleCommand))
            {
                ConsoleCommandsMap[consoleCommand](interpreter, value);

            }
            else
            {
                Console.WriteLine(">Unknown Command");
            }
            

        }

        private static string ParseToCommand(string consoleInput)
        {
            string command;
            command = consoleInput.Substring(0, consoleInput.IndexOf(' '));
            return command;
        }

        private static string ParseToValue(string consoleInput)
        {
            string value;
            value = consoleInput.Substring(consoleInput.IndexOf(' ') + 1);
            return value;
        }

        private static void StartEmulatorCommand(InterpreterInstance interpreter, string value)
        {
            if (interpreter.CurrentCPU.Memory.currentROMPath != null)
            {
                interpreter.IsRunning = true;
                Console.Clear();
                Console.WriteLine($">Runing game from specified {interpreter.CurrentCPU.Memory.currentROMPath} file ROM with CPUFrequency = {interpreter.CurrentCPU.CPUClockRate} Hz enjoy!!!!");
            }
            else
            {
                Console.WriteLine(">You propably should load your cartridge first ^-^");
            }
        }

        private static void LoadRomCommand(InterpreterInstance interpreter, string value)
        {
            if (File.Exists(value))
            {
                interpreter.CurrentCPU.Memory.LoadProgram($@"{value}");
                Console.WriteLine($">Program {interpreter.CurrentCPU.Memory.currentROMPath} loaded!");
            }
            else
            {
                Console.WriteLine(">Unable to load specifed file check if file exists");
            }
        }

        private static void SetDisplayModeCommand(InterpreterInstance interpreter, string value)
        {
            value = value.ToLower();

            if (DisplayModesMap.ContainsKey(value))
            {
                interpreter.Engine.DisplayMode = DisplayModesMap[value];
                Console.WriteLine($"Display Mode : {value}");
            }
            else
            {
                Console.WriteLine(">Wrong value choose [default/fallout/red/blue]");
            }
        }

        private static void DisplayHelpCommand(InterpreterInstance interpreter, string value)
        {
            Console.WriteLine("->Example of invoking command  >LoadRom TETRIS or >SetClockRate 200");
            Console.WriteLine("->Run - to run a program from specified ROM source");
            Console.WriteLine("->LoadRom \"roamtoload\" - specify ROM source");
            Console.WriteLine("->SetClockRate [target frequency in Hz] for example SetCPUFreq 500 sets CPU frequency to 500 HZ (range 200-2000), defualt is 600 ");
            Console.WriteLine("->DisplayMode [default|fallout|blue|red] - Fallout mode on turns colors to green and gray while off is true to orginal Chip-8 mono, defualt is off");
            Console.WriteLine("->Quit - Bye!!");
            Console.WriteLine("->SuperChipMode [on/off] - Allows to run SuperChip-8 programs - not implemented yet so don't bother");
        }

        private static void SetClockRateCommand(InterpreterInstance interpreter, string value)
        {
            int clock = 0;
            bool isGood = false;
            isGood = int.TryParse(value, out clock);
            if (isGood && clock >= 200 && clock <= 2000)
            {
                interpreter.CurrentCPU.CPUClockRate = int.Parse(value);
                Console.WriteLine($">Current cpu frequency = {interpreter.CurrentCPU.CPUClockRate} Hz");
            }
            else
            {
                Console.WriteLine(">Specified value is out of range");
            }
        }

        private static void SuperChipModeCommand(InterpreterInstance interpreter, string value)
        {
            Console.WriteLine(">Not a chance unless i find a good reference for SCHP-48 or MegaChip-8 ");
        }

        private static void QuitCommand(InterpreterInstance interpreter, string value)
        {
            Environment.Exit(0);
        }





    }
}
