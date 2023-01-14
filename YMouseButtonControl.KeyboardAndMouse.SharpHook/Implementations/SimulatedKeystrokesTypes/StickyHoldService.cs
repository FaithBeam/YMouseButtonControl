using System.Collections.Generic;
using System.Linq;
using YMouseButtonControl.DataAccess.Models.Interfaces;
using YMouseButtonControl.KeyboardAndMouse.Interfaces;

namespace YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations.SimulatedKeystrokesTypes;

public class StickyHoldService : IStickyHoldService
{
    private readonly ISimulateKeyService _simulateKeyService;
    private readonly IParseKeysService _parseKeysService;

    public StickyHoldService(ISimulateKeyService simulateKeyService, IParseKeysService parseKeysService)
    {
        _simulateKeyService = simulateKeyService;
        _parseKeysService = parseKeysService;
    }

    public void StickyHold(IButtonMapping mapping, bool pressed)
    {
        if (!pressed) return;

        if (mapping.State)
        {
            StickyHoldRelease(_parseKeysService.ParseKeys(mapping.Keys));
            mapping.State = false;
        }
        else
        {
            StickyHoldPress(_parseKeysService.ParseKeys(mapping.Keys));
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