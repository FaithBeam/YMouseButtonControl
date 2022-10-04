using SharpHook;
using YMouseButtonControl.KeyboardAndMouse.Models;

namespace YMouseButtonControl.KeyboardAndMouse.SharpHook
{
    public class KeyboardSimulator : IKeyboardSimulator
    {
        private EventSimulator _keyboardSimulator;

        public KeyboardSimulator(EventSimulator keyboardSimulator)
        {
            _keyboardSimulator = keyboardSimulator;
        }

        public SimulateKeyboardResult SimulateKeyPress(string key) => new()
            { Result = _keyboardSimulator.SimulateKeyPress(KeyCodeMappings.KeyCodes[key]).ToString() };
        
        public SimulateKeyboardResult SimulateKeyRelease(string key) => new()
            { Result = _keyboardSimulator.SimulateKeyRelease(KeyCodeMappings.KeyCodes[key]).ToString() };
    }
}