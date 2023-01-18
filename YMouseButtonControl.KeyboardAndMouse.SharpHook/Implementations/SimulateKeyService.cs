using System.Collections.Generic;
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
        foreach (var pk in _parseKeysService.ParseKeys(keys))
        {
            SimulateKeyPress(pk.Key);
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
        foreach (var pk in parsed)
        {
            SimulateKeyRelease(pk.Key);
        }
    }

    public void TapKeys(string keys)
    {
        var parsed = _parseKeysService.ParseKeys(keys);
        var stack = new Stack<ParsedKey>();
        
        foreach (var pk in parsed)
        {
            if (stack.Count > 0 && !stack.Peek().IsModifier)
            {
                // Pop all items in stack
                while (stack.TryPop(out var poppedPk))
                {
                    SimulateKeyRelease(poppedPk.Key);
                }
            }
            
            stack.Push(pk);
            SimulateKeyPress(pk.Key);
        }
        
        // Pop all items in stack
        while (stack.TryPop(out var poppedPk))
        {
            SimulateKeyRelease(poppedPk.Key);
        }
    }
}