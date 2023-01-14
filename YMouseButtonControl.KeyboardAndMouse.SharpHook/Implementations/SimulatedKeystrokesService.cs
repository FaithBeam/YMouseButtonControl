using YMouseButtonControl.DataAccess.Models.Implementations.SimulatedKeystrokesTypes;
using YMouseButtonControl.DataAccess.Models.Interfaces;
using YMouseButtonControl.KeyboardAndMouse.Interfaces;

namespace YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations;

public class SimulatedKeystrokesService : ISimulatedKeystrokesService
{
    private readonly ISimulateKeyService _simulateKeyService;
    private readonly IParseKeysService _parseKeysService;
    private readonly IStickyHoldService _stickyHoldService;

    public SimulatedKeystrokesService(ISimulateKeyService simulateKeyService, IParseKeysService parseKeysService,
        IStickyHoldService stickyHoldService)
    {
        _simulateKeyService = simulateKeyService;
        _parseKeysService = parseKeysService;
        _stickyHoldService = stickyHoldService;
    }

    public void SimulatedKeystrokes(IButtonMapping buttonMapping, bool pressed)
    {
        switch (buttonMapping.SimulatedKeystrokesType)
        {
            case StickyHoldActionType:
                _stickyHoldService.StickyHold(buttonMapping, pressed);
                break;
        }
    }
}