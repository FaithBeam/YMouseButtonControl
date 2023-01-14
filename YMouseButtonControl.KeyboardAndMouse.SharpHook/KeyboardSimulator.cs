using System.Collections.Generic;
using System.Linq;
using System.Threading;
using SharpHook;
using YMouseButtonControl.DataAccess.Models.Implementations.SimulatedKeystrokesTypes;
using YMouseButtonControl.DataAccess.Models.Interfaces;
using YMouseButtonControl.KeyboardAndMouse.Interfaces;
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
            switch (buttonMapping.SimulatedKeystrokesType)
            {
                case StickyHoldActionType:
                    StickyHold(buttonMapping, pressed);
                    break;
            }
        }

        private void StickyHold(IButtonMapping mapping, bool pressed)
        {
            if (!pressed) return;

            if (mapping.State)
            {
                StickyHoldRelease(ParseKeysService.ParseKeys(mapping.Keys));
                mapping.State = false;
            }
            else
            {
                StickyHoldPress(ParseKeysService.ParseKeys(mapping.Keys));
                mapping.State = true;
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