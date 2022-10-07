
using YMouseButtonControl.KeyboardAndMouse.Models;

namespace YMouseButtonControl.KeyboardAndMouse;

public interface IKeyboardSimulator
{
    SimulateKeyboardResult SimulateKeyPress(string key);
    SimulateKeyboardResult SimulateKeyRelease(string key);
}