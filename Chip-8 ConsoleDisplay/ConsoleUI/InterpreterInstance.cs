using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Architecture;
using SDLLayer;

namespace Chip_8_ConsoleDisplay.ConsoleUI
{
    public class InterpreterInstance
    {
        private bool isRunning;

        public bool IsRunning
        {
            get { return isRunning; }
            set { isRunning = value; }
        }

        public DisplayMode DisplayScheme { get; set; } = DisplayMode.DefaultMode;

        public IEngine Engine;

        public CPU CurrentCPU;

        public InterpreterInstance(CPU cpu, IEngine engine )
        {
            CurrentCPU = cpu;
            Engine = engine;
        }

        public void Run()
        {
            this.GreetTheUser();

            while (!isRunning)
            {
                this.ProcessCommands();
            }

            SetUp();

            while (IsRunning)
            {
                EmulationCycle();
            }
        
            CleanUp();
        }


        private void SetUp()
        {
            Engine.OpenWindow();
            Engine.TriesToQuitWhileWaitingEvent += Engine_TriesToQuitWhileWaitingEvent;
            Engine.SetColorScheme(DisplayScheme);
        }

        private void Engine_TriesToQuitWhileWaitingEvent(object sender, bool e)
        {
            IsRunning = false;
        }

        private void EmulationCycle()
        {
            CurrentCPU.FullCycle();
            Engine.Render();
            Engine.HandleEvents(ref isRunning);
        }

        private void CleanUp()
        {
            Engine.TriesToQuitWhileWaitingEvent -= Engine_TriesToQuitWhileWaitingEvent;
            Engine.Quit();
            CurrentCPU.Reset();
        }


    }
}
