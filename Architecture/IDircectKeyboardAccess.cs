using System;

namespace Architecture
{
    public interface IDircectKeyboardAccess
    {
        bool AwaitsForKeypress { get; set; }
        bool[] KeyState { get; set; }

        event EventHandler<bool> WaitForKeypressEvent;
    }
}