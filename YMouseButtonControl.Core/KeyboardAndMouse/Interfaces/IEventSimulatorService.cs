using SharpHook.Native;
using YMouseButtonControl.Core.KeyboardAndMouse.Models;

namespace YMouseButtonControl.Core.KeyboardAndMouse.Interfaces;

public interface IEventSimulatorService
{
    SimulateKeyboardResult SimulateKeyPress(string? key);
    SimulateKeyboardResult SimulateKeyRelease(string? key);

    /// <summary>
    /// Press then release
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    SimulateKeyboardResult SimulateKeyTap(string? key);

    /// <summary>
    /// Keys to be pressed in order.
    /// </summary>
    /// <param name="keys">Keys to be pressed</param>
    void PressKeys(string? keys);

    /// <summary>
    /// Keys will be released in the reversed order they come. For example, sending abc to this method will release cba
    /// in that order.
    /// </summary>
    /// <param name="keys">Keys to be released</param>
    void ReleaseKeys(string? keys);

    void TapKeys(string? keys);
    void SimulateMousePress(MouseButton mb);
    void SimulateMouseRelease(MouseButton mb);
}
