using System;
using Architecture;
using DisplayLibrary;
using Chip_8_ConsoleDisplay.ConsoleUI;

namespace Chip_8_ConsoleDisplay
{
    class Program
    {
      
        static void Main(string[] args)
        {                     
            CPU cpu = new CPU();
            SDLWindowDisplay sdl = new SDLWindowDisplay(cpu, cpu.Display);

            InterpreterInstance interpreter = new InterpreterInstance(cpu, sdl);

            while (true)
            {     
                interpreter.Run();
            }

        }


     

    }
}
