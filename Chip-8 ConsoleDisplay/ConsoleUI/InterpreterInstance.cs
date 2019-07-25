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
        }

        private void Engine_TriesToQuitWhileWaitingEvent(object sender, bool e)
        {
            IsRunning = false;
        }

        private void EmulationCycle()
        {
            // Execute x opcodes where x = clock rate / frames per second
            for (int i = 0; i < CurrentCPU.CPUClockRate / 60; i++)
            {
                CurrentCPU.FullCycle();
            }

            CurrentCPU.DecrementeTimers();
            Engine.HandleEvents(ref isRunning);
            Engine.Render();
            Thread.Sleep(TimeSpan.FromMilliseconds(16));
        }

        private void CleanUp()
        {
            Engine.TriesToQuitWhileWaitingEvent -= Engine_TriesToQuitWhileWaitingEvent;
            Engine.Quit();
            CurrentCPU.Reset();
        }


    }
}
