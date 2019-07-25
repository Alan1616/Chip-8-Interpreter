using System;
namespace DisplayLibrary
{
    public interface IEngine
    {
        event EventHandler<bool> TriesToQuitWhileWaitingEvent;

        DisplayMode DisplayMode { get; set; }

        void OpenWindow();
        void HandleEvents(ref bool isRunning);
        void Quit();
        void Render();
    }
}