using System.Collections.Generic;
using SharpHook;
using YMouseButtonControl.KeyboardAndMouse.Models;

namespace YMouseButtonControl.KeyboardAndMouse.SharpHook
{
    public class KeyboardSimulator : IKeyboardSimulator
    {
        private readonly EventSimulator _keyboardSimulator;

        public KeyboardSimulator(EventSimulator keyboardSimulator)
        {
            _keyboardSimulator = keyboardSimulator;
        }

        private SimulateKeyboardResult SimulateKeyPress(string key) => new()
            { Result = _keyboardSimulator.SimulateKeyPress(KeyCodeMappings.KeyCodes[key]).ToString() };
        
        private SimulateKeyboardResult SimulateKeyRelease(string key) => new()
            { Result = _keyboardSimulator.SimulateKeyRelease(KeyCodeMappings.KeyCodes[key]).ToString() };
        
        public void SimulatedKeystrokesReleased(IEnumerable<char> keys)
        {
            foreach (var c in keys)
            {
                SimulateKeyRelease(c.ToString());
            }
        }

        public void SimulatedKeystrokesPressed(string keys)
        {
            foreach (var c in keys)
            {
                SimulateKeyPress(c.ToString());
            }
        }
        
    }
}