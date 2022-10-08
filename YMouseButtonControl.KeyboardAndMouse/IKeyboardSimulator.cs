
using System.Collections.Generic;

namespace YMouseButtonControl.KeyboardAndMouse;

public interface IKeyboardSimulator
{
    void SimulatedKeystrokesReleased(IEnumerable<char> keys);
    void SimulatedKeystrokesPressed(string keys);
}