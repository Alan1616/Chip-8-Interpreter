﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Architecture;
using SDL2;

namespace SDLLayer
{
    public class SDLWindowDisplay : IEngine
    {
        private const float FRAMES_PER_SECOND = 60F;
        private const float FRAME_TIME = 1000F / FRAMES_PER_SECOND;

        private bool FalloutModeRender { get; set; }
        private IntPtr window;
        private IntPtr renderer;
        private readonly IDircectKeyboardAccess keyboardAccess;
        private readonly IDirectDisplayAccess displayAccess;
        private uint[] PixelsData = new uint[64 * 32];
        private readonly Dictionary<SDL.SDL_Keycode, byte> keyboardMap;
        public event EventHandler<bool> TriesToQuitWhileWaitingEvent;
        private Stopwatch frameTimer = new Stopwatch();

        public SDLWindowDisplay(IDircectKeyboardAccess keyboardSource,IDirectDisplayAccess displaySource, bool modeFlag)
        {
    
            keyboardAccess = keyboardSource;
            displayAccess = displaySource;

            FalloutModeRender = modeFlag;
            keyboardAccess.WaitForKeypressEvent += keyboardAccess_WaitForKeypressEvent;

            //window = SDL.SDL_CreateWindow("Chip-8 Emulator", SDL.SDL_WINDOWPOS_CENTERED, SDL.SDL_WINDOWPOS_CENTERED, 800, 600, SDL.SDL_WindowFlags.SDL_WINDOW_RESIZABLE);
            //renderer = SDL.SDL_CreateRenderer(window, -1, 0);
            //SDL.SDL_SetRenderDrawColor(renderer, 0, 0, 0, 0);

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

        public void OpenWindow()
        {
            if (SDL.SDL_Init(SDL.SDL_INIT_EVERYTHING) < 0)
            {
                Console.WriteLine("SDL failed to init.");
            }
            window = SDL.SDL_CreateWindow("Chip-8 Emulator", SDL.SDL_WINDOWPOS_CENTERED, SDL.SDL_WINDOWPOS_CENTERED, 800, 600, SDL.SDL_WindowFlags.SDL_WINDOW_RESIZABLE);
            renderer = SDL.SDL_CreateRenderer(window, -1, 0);
            SDL.SDL_SetRenderDrawColor(renderer, 0, 0, 0, 0);
        }

        private void keyboardAccess_WaitForKeypressEvent(object sender, bool e)
        {
            while (keyboardAccess.AwaitsForKeypress)
            {
                SDL.SDL_Event ev;
                SDL.SDL_PollEvent(out ev);
                if (ev.type == SDL.SDL_EventType.SDL_KEYDOWN && keyboardMap.ContainsKey(ev.key.keysym.sym))
                {
                    keyboardAccess.KeyState[keyboardMap[ev.key.keysym.sym]] = true;
                    keyboardAccess.AwaitsForKeypress = false;
                }
                if (ev.type == SDL.SDL_EventType.SDL_QUIT)
                {
                    keyboardAccess.AwaitsForKeypress = false;
                    TriesToQuitWhileWaitingEvent?.Invoke(this, keyboardAccess.AwaitsForKeypress);
                }
                Render();
            }
        }
        public  void HandleEvents(ref bool isRunning)
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
                        keyboardAccess.KeyState[keyboardMap[ev.key.keysym.sym]] = true;
                    break;
                case SDL.SDL_EventType.SDL_KEYUP:
                    if (keyboardMap.ContainsKey(ev.key.keysym.sym))
                        keyboardAccess.KeyState[keyboardMap[ev.key.keysym.sym]] = false;
                    break;
                default:
                    break;
            }
        }
        public void Render()
        {
            if (!frameTimer.IsRunning)
            {
                frameTimer.Start();
            }

            if (frameTimer.Elapsed.TotalMilliseconds >= FRAME_TIME)
            {
                UpdatePixelData();
                RenderSingleFrame();

                frameTimer.Reset();
            }

    }

        private void RenderSingleFrame()
        {
            //setup 
            GCHandle displayHande = GCHandle.Alloc(PixelsData, GCHandleType.Pinned);

            IntPtr sdlSurface = SDL.SDL_CreateRGBSurfaceFrom(displayHande.AddrOfPinnedObject(), 64, 32, 32, 256, 0xff000000, 0x00ff0000, 0x0000ff00, 0x000000ff);
            IntPtr sdlTexture = SDL.SDL_CreateTextureFromSurface(renderer, sdlSurface);
        
            //add to render
            SDL.SDL_RenderClear(renderer);
            SDL.SDL_RenderCopy(renderer, sdlTexture, IntPtr.Zero, IntPtr.Zero);
            SDL.SDL_RenderPresent(renderer);

            //free memory
            SDL.SDL_DestroyTexture(sdlTexture);
            SDL.SDL_FreeSurface(sdlSurface);
            displayHande.Free();
        }

        private void UpdatePixelData()
        {
            for (int i = 0; i < 32; i++)
            {
                for (int j = 0; j < 64; j++)
                {
                    if (displayAccess.PixelsState[j + i * 64])
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
            keyboardAccess.WaitForKeypressEvent -= keyboardAccess_WaitForKeypressEvent;
            SDL.SDL_DestroyWindow(window);
            SDL.SDL_DestroyRenderer(renderer);
            SDL.SDL_Quit();
        }
    }
}
