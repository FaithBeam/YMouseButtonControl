using SharpHook;
using YMouseButtonControl.KeyboardAndMouse.Interfaces;
using YMouseButtonControl.KeyboardAndMouse.Models;

namespace YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations;

public class SimulateKeyService : ISimulateKeyService
{
    private readonly EventSimulator _keyboardSimulator;

    public SimulateKeyService(EventSimulator keyboardSimulator)
    {
        _keyboardSimulator = keyboardSimulator;
    }
    
    public SimulateKeyboardResult SimulateKeyPress(string key) => new()
        { Result = _keyboardSimulator.SimulateKeyPress(KeyCodeMappings.KeyCodes[key]).ToString() };
        
    public SimulateKeyboardResult SimulateKeyRelease(string key) => new()
        { Result = _keyboardSimulator.SimulateKeyRelease(KeyCodeMappings.KeyCodes[key]).ToString() };
}