using System.Collections.Generic;
using System.Linq;
using SharpHook;
using YMouseButtonControl.KeyboardAndMouse.Interfaces;
using YMouseButtonControl.KeyboardAndMouse.Models;

namespace YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations;

public class SimulateKeyService : ISimulateKeyService
{
    private readonly IEventSimulator _keyboardSimulator;

    public SimulateKeyService(IEventSimulator keyboardSimulator)
    {
        _keyboardSimulator = keyboardSimulator;
    }
    
    public SimulateKeyboardResult SimulateKeyPress(string key) => new()
        { Result = _keyboardSimulator.SimulateKeyPress(KeyCodeMappings.KeyCodes[key]).ToString() };
        
    public SimulateKeyboardResult SimulateKeyRelease(string key) => new()
        { Result = _keyboardSimulator.SimulateKeyRelease(KeyCodeMappings.KeyCodes[key]).ToString() };

    /// <summary>
    /// Press then release
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public SimulateKeyboardResult SimulateKeyTap(string key)
    {
        var keyCode = KeyCodeMappings.KeyCodes[key];
        _keyboardSimulator.SimulateKeyPress(keyCode);
        _keyboardSimulator.SimulateKeyRelease(keyCode);
        return new SimulateKeyboardResult { Result = "Success" };
    }
    
    /// <summary>
    /// Keys to be pressed in order.
    /// </summary>
    /// <param name="keys">Keys to be pressed</param>
    public void PressKeys(IEnumerable<string> keys)
    {
        foreach (var c in keys)
        {
            SimulateKeyPress(c);
        }
    }

    /// <summary>
    /// Keys will be released in the reversed order they come. For example, sending abc to this method will release cba
    /// in that order.
    /// </summary>
    /// <param name="keys">Keys to be released</param>
    public void ReleaseKeys(IEnumerable<string> keys)
    {
        foreach (var c in keys.Reverse())
        {
            SimulateKeyRelease(c);
        }
    }

    public void TapKeys(IEnumerable<string> keys)
    {
        foreach (var k in keys)
        {
            SimulateKeyTap(k);
        }
    }
}