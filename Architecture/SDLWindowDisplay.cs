using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using SDL2;

namespace Architecture
{
    public class SDLWindowDisplay
    {

        //bool isRunning = false;
        private IntPtr window;
        private IntPtr renderer;
        private IntPtr sdlSurface;
        private IntPtr sdlTexture;
        private CPU usedCPU;

        public SDLWindowDisplay(CPU cpu)
        {
            if (SDL.SDL_Init(SDL.SDL_INIT_EVERYTHING) < 0)
            {
                Console.WriteLine("SDL failed to init.");
            }
            usedCPU = cpu;

            window = SDL.SDL_CreateWindow("Chip-8 Emulator", SDL.SDL_WINDOWPOS_CENTERED, SDL.SDL_WINDOWPOS_CENTERED, 800, 600, SDL.SDL_WindowFlags.SDL_WINDOW_RESIZABLE);
            renderer = SDL.SDL_CreateRenderer(window, -1, 0);
            SDL.SDL_SetRenderDrawColor(renderer, 0, 0, 0, 0);

            sdlSurface = IntPtr.Zero;
            sdlTexture = IntPtr.Zero;
        }

        public  void HandleEvents(ref bool isRunning )
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
                    usedCPU.keyState[mapKey(ev)] = true;
                    //Console.WriteLine(ev.key.keysym.sym.ToString());
                    break;
                case SDL.SDL_EventType.SDL_KEYUP:
                    usedCPU.keyState[mapKey(ev)] = false;
                    break;

                default:
                    break;
            }
        }

        public void render()
        {
            GCHandle displayHande = GCHandle.Alloc(usedCPU.Display.PixelsData, GCHandleType.Pinned);

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

        private static int mapKey(SDL.SDL_Event ev)
        {
            int output = 0;

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

        public void Quit()
        {
            SDL.SDL_DestroyWindow(window);
            SDL.SDL_DestroyRenderer(renderer);
            SDL.SDL_Quit();
        }
    }
}
