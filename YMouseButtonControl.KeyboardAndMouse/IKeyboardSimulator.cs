
using YMouseButtonControl.KeyboardAndMouse.Models;

namespace YMouseButtonControl.KeyboardAndMouse.SharpHook;

public interface IKeyboardSimulator
{
    SimulateKeyboardResult SimulateKeyPress(string key);
    SimulateKeyboardResult SimulateKeyRelease(string key);
}