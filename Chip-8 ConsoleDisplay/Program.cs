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
            bool isRunning = false;

            if (SDL.SDL_Init(SDL.SDL_INIT_EVERYTHING) < 0)
            {
                Console.WriteLine("SDL failed to init.");
            }

            IntPtr window =  SDL.SDL_CreateWindow("Chip-8 Emulator", SDL.SDL_WINDOWPOS_CENTERED, SDL.SDL_WINDOWPOS_CENTERED, 640, 320, SDL.SDL_WindowFlags.SDL_WINDOW_RESIZABLE);
            IntPtr renderer = SDL.SDL_CreateRenderer(window, -1, 0);

            SDL.SDL_SetRenderDrawColor(renderer, 0, 0, 0, 0);

            isRunning = true;


            CPU c1 = new CPU();
            c1.m1.LoadProgram(@"INVADERS");
            IntPtr sdlSurface = IntPtr.Zero;
            IntPtr sdlTexture = IntPtr.Zero;

            //do
            //{
            //    c1.FullCycle();
            //} while (!signaled);

            while (isRunning)
            {
                //    //Console.WriteLine($"{ c1.FullCycle():X4}");
                HandleEvents(ref isRunning,c1);
                c1.FullCycle();

                GCHandle displayHande = GCHandle.Alloc(c1.Display.PixelsData, GCHandleType.Pinned);

                if (sdlTexture != IntPtr.Zero) SDL.SDL_DestroyTexture(sdlTexture);

                sdlSurface = SDL.SDL_CreateRGBSurfaceFrom(displayHande.AddrOfPinnedObject(), 64, 32, 32, 256, 0xff000000, 0x00ff0000, 0x0000ff00, 0x000000ff);
                sdlTexture = SDL.SDL_CreateTextureFromSurface(renderer, sdlSurface);
                SDL.SDL_FreeSurface(sdlSurface);
                displayHande.Free();
                //add to render
                SDL.SDL_RenderClear(renderer);
                SDL.SDL_RenderCopy(renderer, sdlTexture, IntPtr.Zero, IntPtr.Zero);
                SDL.SDL_RenderPresent(renderer);

                //Thread.Sleep(1);


                //Render(renderer, c1, sdlSurface, sdlTexture);
                //    //Thread.Sleep(1);
                //    //Console.WriteLine($"V[6]={c1.V[6]}");
                //    //Console.WriteLine($"V[7]={c1.V[7]}");
            }

            //BinaryReader b1 = new BinaryReader(File.Open("Chip8 Picture.ch8", FileMode.Open), System.Text.Encoding.BigEndianUnicode);
            //while (b1.BaseStream.Position < b1.BaseStream.Length)
            //{
            //    ushort opcode = ConvertUInt16ToBigEndian((b1.ReadUInt16()));
            //    Console.WriteLine($"{opcode:X4}");

            //    //c1.ExecuteOpcode(opcode);
            //}

            //b1.Close();


            SDL.SDL_DestroyWindow(window);
            SDL.SDL_DestroyRenderer(renderer);
            SDL.SDL_Quit();


            Console.ReadKey();
        }

        private static void HandleEvents(ref bool isRunning, CPU c1)
        {
            SDL.SDL_Event ev;
            SDL.SDL_PollEvent(out ev);
            int key;
            switch (ev.type)
            {
                case SDL.SDL_EventType.SDL_QUIT:
                    isRunning = false;
                    break;
                case SDL.SDL_EventType.SDL_KEYDOWN:
                    c1.keyState[mapKey(ev)] = true;
                    //Console.WriteLine(ev.key.keysym.sym.ToString());
                    break;
                case SDL.SDL_EventType.SDL_KEYUP:
                    c1.keyState[mapKey(ev)] = false;
                    break;

                default:
                    break;
            }
        }


        private static int mapKey(SDL.SDL_Event ev)
        {
            int output= 0;

            if (ev.key.keysym.sym == SDL.SDL_Keycode.SDLK_0)
            {
                output = 0;
            }
            else if (ev.key.keysym.sym == SDL.SDL_Keycode.SDLK_1)
            {
                output = 1;
            }
            else if (ev.key.keysym.sym == SDL.SDL_Keycode.SDLK_2)
            {
                output = 2;
            }
            else if (ev.key.keysym.sym == SDL.SDL_Keycode.SDLK_3)
            {
                output = 3;
            }
            else if (ev.key.keysym.sym == SDL.SDL_Keycode.SDLK_4)
            {
                output = 4;
            }
            else if (ev.key.keysym.sym == SDL.SDL_Keycode.SDLK_5)
            {
                output = 5;
            }
            else if (ev.key.keysym.sym == SDL.SDL_Keycode.SDLK_6)
            {
                output = 6;
            }

            return output;
        }


        private static void Render(IntPtr renderer, CPU c1, IntPtr sdlSurface, IntPtr sdlTexture)
        {
            GCHandle displayHande = GCHandle.Alloc(c1.Display.PixelsData, GCHandleType.Pinned);

            if (sdlTexture != IntPtr.Zero) SDL.SDL_DestroyTexture(sdlTexture);

            sdlSurface = SDL.SDL_CreateRGBSurfaceFrom(displayHande.AddrOfPinnedObject(), 64, 32, 32, 256, 0xff000000, 0x00ff0000, 0x0000ff00, 0x000000ff);
            sdlTexture = SDL.SDL_CreateTextureFromSurface(renderer, sdlSurface);
            SDL.SDL_FreeSurface(sdlSurface);
            displayHande.Free();
            //add to render
            SDL.SDL_RenderClear(renderer);
            SDL.SDL_RenderCopy(renderer, sdlTexture, IntPtr.Zero, IntPtr.Zero);
            SDL.SDL_RenderPresent(renderer);
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
