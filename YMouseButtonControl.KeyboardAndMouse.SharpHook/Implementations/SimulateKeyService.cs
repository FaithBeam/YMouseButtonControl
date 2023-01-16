using SharpHook;
using YMouseButtonControl.KeyboardAndMouse.Interfaces;
using YMouseButtonControl.KeyboardAndMouse.Models;

namespace YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations;

public class SimulateKeyService : ISimulateKeyService
{
    private readonly IEventSimulator _keyboardSimulator;
    private readonly IParseKeysService _parseKeysService;

    public SimulateKeyService(IEventSimulator keyboardSimulator, IParseKeysService parseKeysService)
    {
        _keyboardSimulator = keyboardSimulator;
        _parseKeysService = parseKeysService;
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
    public void PressKeys(string keys)
    {
        foreach (var c in _parseKeysService.ParseKeys(keys))
        {
            SimulateKeyPress(c);
        }
    }

    /// <summary>
    /// Keys will be released in the reversed order they come. For example, sending abc to this method will release cba
    /// in that order.
    /// </summary>
    /// <param name="keys">Keys to be released</param>
    public void ReleaseKeys(string keys)
    {
        var parsed = _parseKeysService.ParseKeys(keys);
        parsed.Reverse();
        foreach (var c in parsed)
        {
            SimulateKeyRelease(c);
        }
    }

    public void TapKeys(string keys)
    {
        foreach (var k in _parseKeysService.ParseKeys(keys))
        {
            SimulateKeyTap(k);
        }
    }
}