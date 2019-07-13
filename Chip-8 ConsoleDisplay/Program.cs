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
using Chip_8_ConsoleDisplay.ConsoleUI;

namespace Chip_8_ConsoleDisplay
{
    class Program
    {
      
        static void Main(string[] args)
        {                     
            CPU cpu = new CPU();
            SDLWindowDisplay sdl = new SDLWindowDisplay(cpu, cpu.Display, true);

            InterpreterInstance interpreter = new InterpreterInstance(cpu, sdl);

            while (true)
            {
                interpreter.GreetTheUser();

                while (!interpreter.IsRunning)
                {
                    interpreter.ProcessCommands();
                }
             
                interpreter.Run();

            }

        }


     

    }
}
