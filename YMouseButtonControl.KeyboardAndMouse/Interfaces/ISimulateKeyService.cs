using YMouseButtonControl.KeyboardAndMouse.Models;

namespace YMouseButtonControl.KeyboardAndMouse.Interfaces;

public interface ISimulateKeyService
{
    SimulateKeyboardResult SimulateKeyPress(string key);
    SimulateKeyboardResult SimulateKeyRelease(string key);
}