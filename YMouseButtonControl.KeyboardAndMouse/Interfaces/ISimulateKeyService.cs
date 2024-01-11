using System.Collections.Generic;
using SharpHook.Native;
using YMouseButtonControl.DataAccess.Models.Interfaces;
using YMouseButtonControl.KeyboardAndMouse.Models;

namespace YMouseButtonControl.KeyboardAndMouse.Interfaces;

public interface ISimulateKeyService
{
    SimulateKeyboardResult SimulateKeyPress(KeyCode key);
    SimulateKeyboardResult SimulateKeyRelease(KeyCode key);
    UioHookResult SimulateMousePress(MouseButton mb);
    UioHookResult SimulateMouseRelease(MouseButton mb);

    /// <summary>
    /// Press then release
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    SimulateKeyboardResult SimulateKeyTap(string key);

    void PressKeys(IEnumerable<IParsedEvent> events);

    /// <summary>
    /// Keys will be released in the reversed order they come. For example, sending abc to this method will release cba
    /// in that order.
    /// </summary>
    void ReleaseKeys(IEnumerable<IParsedEvent> events);

    void TapKeys(IEnumerable<IParsedEvent> events);
}