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
        private bool FalloutModeRender { get; set; }
        private IntPtr window;
        private IntPtr renderer;
        private IntPtr sdlSurface;
        private IntPtr sdlTexture;
        private CPU usedCPU;
        private uint[] PixelsData = new uint[64 * 32];
        private readonly Dictionary<SDL.SDL_Keycode, byte> keyboardMap;

        public SDLWindowDisplay(CPU cpu, bool modeFlag)
        {
            if (SDL.SDL_Init(SDL.SDL_INIT_EVERYTHING) < 0)
            {
                Console.WriteLine("SDL failed to init.");
            }
            usedCPU = cpu;
            FalloutModeRender = modeFlag;

            window = SDL.SDL_CreateWindow("Chip-8 Emulator", SDL.SDL_WINDOWPOS_CENTERED, SDL.SDL_WINDOWPOS_CENTERED, 800, 600, SDL.SDL_WindowFlags.SDL_WINDOW_RESIZABLE);
            renderer = SDL.SDL_CreateRenderer(window, -1, 0);
            SDL.SDL_SetRenderDrawColor(renderer, 0, 0, 0, 0);

            sdlSurface = IntPtr.Zero;
            sdlTexture = IntPtr.Zero;
            keyboardMap = new Dictionary<SDL.SDL_Keycode, byte>
            {
                { SDL.SDL_Keycode.SDLK_x,0 },
                { SDL.SDL_Keycode.SDLK_1,1 },
                { SDL.SDL_Keycode.SDLK_2,2 },
                { SDL.SDL_Keycode.SDLK_3,3 },
                { SDL.SDL_Keycode.SDLK_q,4 },
                { SDL.SDL_Keycode.SDLK_w,5 },
                { SDL.SDL_Keycode.SDLK_e,6 },
                { SDL.SDL_Keycode.SDLK_a,7 },
                { SDL.SDL_Keycode.SDLK_s,8 },
                { SDL.SDL_Keycode.SDLK_d,9 },
                { SDL.SDL_Keycode.SDLK_z,0xA },
                { SDL.SDL_Keycode.SDLK_c,0xB },
                { SDL.SDL_Keycode.SDLK_4,0xC},
                { SDL.SDL_Keycode.SDLK_r,0xD },
                { SDL.SDL_Keycode.SDLK_f,0xE },
                { SDL.SDL_Keycode.SDLK_v,0xF },
            };
        }

        public  void HandleEvents(ref bool isRunning )
        {
            SDL.SDL_Event ev;
            SDL.SDL_PollEvent(out ev);
            switch (ev.type)
            {
                case SDL.SDL_EventType.SDL_QUIT:
                    isRunning = false;
                    break;
                case SDL.SDL_EventType.SDL_KEYDOWN:
                    if (keyboardMap.ContainsKey(ev.key.keysym.sym))
                        usedCPU.keyState[keyboardMap[ev.key.keysym.sym]] = true;
                    //Console.WriteLine(ev.key.keysym.sym.ToString());
                    break;
                case SDL.SDL_EventType.SDL_KEYUP:
                    if (keyboardMap.ContainsKey(ev.key.keysym.sym))
                        usedCPU.keyState[keyboardMap[ev.key.keysym.sym]] = false;
                    break;
                default:
                    break;
            }
        }

        public void render()
        {
            UpdatePixelData();
            GCHandle displayHande = GCHandle.Alloc(PixelsData, GCHandleType.Pinned);

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

        public void UpdatePixelData()
        {
            for (int i = 0; i < 32; i++)
            {
                for (int j = 0; j < 64; j++)
                {
                    if (usedCPU.Display.PixelsState[j + i * 64])
                    {
                        if(FalloutModeRender)
                            PixelsData[j + i * 64] = 0x003300FF;
                        else
                            PixelsData[j + i * 64] = 0xFFFFFFFF;
                    }
                    else
                    {
                        if (FalloutModeRender)
                            PixelsData[j + i * 64] = 0x9CA89CFF;
                        else
                            PixelsData[j + i * 64] = 0x00000000;    
                    }
                }
            }
        }

        public void Quit()
        {
            SDL.SDL_DestroyWindow(window);
            SDL.SDL_DestroyRenderer(renderer);
            SDL.SDL_Quit();
        }
    }
}
