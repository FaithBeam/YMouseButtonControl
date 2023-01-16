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
            _simulateKeyService.ReleaseKeys(_parseKeysService.ParseKeys(mapping.Keys));
            mapping.State = false;
        }
        else
        {
            _simulateKeyService.PressKeys(_parseKeysService.ParseKeys(mapping.Keys));
            mapping.State = true;
        }
    }
}