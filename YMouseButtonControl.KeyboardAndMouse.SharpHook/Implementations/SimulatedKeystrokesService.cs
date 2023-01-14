using System.Collections.Generic;
using System.Linq;
using YMouseButtonControl.DataAccess.Models.Implementations.SimulatedKeystrokesTypes;
using YMouseButtonControl.DataAccess.Models.Interfaces;
using YMouseButtonControl.KeyboardAndMouse.Interfaces;

namespace YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations;

public class SimulatedKeystrokesService : ISimulatedKeystrokesService
{
    private readonly ISimulateKeyService _simulateKeyService;

    public SimulatedKeystrokesService(ISimulateKeyService simulateKeyService)
    {
        _simulateKeyService = simulateKeyService;
    }

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
            _simulateKeyService.SimulateKeyPress(c);
        }
    }

    private void StickyHoldRelease(IEnumerable<string> keys)
    {
        foreach (var c in keys.Reverse())
        {
            _simulateKeyService.SimulateKeyRelease(c);
        }
    }
}