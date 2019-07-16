using System;
namespace SDLLayer
{
    public interface IEngine
    {
        event EventHandler<bool> TriesToQuitWhileWaitingEvent;

        void OpenWindow();
        void HandleEvents(ref bool isRunning);
        void Quit();
        void Render();
        void SetColorScheme(DisplayMode DisplayScheme);
    }
}