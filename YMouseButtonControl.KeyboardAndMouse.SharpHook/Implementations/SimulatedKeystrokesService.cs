using YMouseButtonControl.DataAccess.Models.Implementations.SimulatedKeystrokesTypes;
using YMouseButtonControl.DataAccess.Models.Interfaces;
using YMouseButtonControl.KeyboardAndMouse.Enums;
using YMouseButtonControl.KeyboardAndMouse.Interfaces;

namespace YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations;

public class SimulatedKeystrokesService : ISimulatedKeystrokesService
{
    private readonly ISimulateKeyService _simulateKeyService;
    private readonly IParseKeysService _parseKeysService;
    private readonly IStickyHoldService _stickyHoldService;
    private readonly IAsMouseButtonPressedService _asMouseButtonPressedService;
    private readonly IAsMouseButtonReleasedService _asMouseButtonReleasedService;

    public SimulatedKeystrokesService(ISimulateKeyService simulateKeyService, IParseKeysService parseKeysService,
        IStickyHoldService stickyHoldService, IAsMouseButtonPressedService asMouseButtonPressedService,
        IAsMouseButtonReleasedService asMouseButtonReleasedService)
    {
        _simulateKeyService = simulateKeyService;
        _parseKeysService = parseKeysService;
        _stickyHoldService = stickyHoldService;
        _asMouseButtonPressedService = asMouseButtonPressedService;
        _asMouseButtonReleasedService = asMouseButtonReleasedService;
    }

    public void SimulatedKeystrokes(IButtonMapping buttonMapping, MouseButtonState state)
    {
        switch (buttonMapping.SimulatedKeystrokesType)
        {
            case MouseButtonPressedActionType:
                _asMouseButtonPressedService.AsMouseButtonPressed(buttonMapping, state);
                break;
            case MouseButtonReleasedActionType:
                _asMouseButtonReleasedService.AsMouseButtonReleased(buttonMapping, state);
                break;
            case StickyHoldActionType:
                _stickyHoldService.StickyHold(buttonMapping, state);
                break;
        }
    }
}