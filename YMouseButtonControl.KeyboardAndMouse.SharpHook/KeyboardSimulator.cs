using System.Collections.Generic;
using System.Linq;
using System.Threading;
using SharpHook;
using YMouseButtonControl.DataAccess.Models.Implementations.SimulatedKeystrokesTypes;
using YMouseButtonControl.DataAccess.Models.Interfaces;
using YMouseButtonControl.KeyboardAndMouse.Models;
using YMouseButtonControl.KeyboardAndMouse.SharpHook.Services;

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

        public void SimulatedKeystrokes(IButtonMapping buttonMapping, bool pressed)
        {
            if (buttonMapping.SimulatedKeystrokesType is not StickyHoldActionType) return;
            if (!pressed) return;
            
            if (buttonMapping.State)
            {
                StickyHoldRelease(ParseKeysService.ParseKeys(buttonMapping.Keys));
                buttonMapping.State = false;
            }
            else
            {
                StickyHoldPress(ParseKeysService.ParseKeys(buttonMapping.Keys));
                buttonMapping.State = true;
            }
        }
        
        private void StickyHoldPress(IEnumerable<string> keys)
        {
            foreach (var c in keys)
            {
                SimulateKeyPress(c);
            }
        }

        private void StickyHoldRelease(IEnumerable<string> keys)
        {
            foreach (var c in keys.Reverse())
            {
                SimulateKeyRelease(c);
            }
        }
    }
}