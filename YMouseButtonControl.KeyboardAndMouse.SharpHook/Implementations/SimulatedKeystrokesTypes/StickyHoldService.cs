using YMouseButtonControl.DataAccess.Models.Interfaces;
using YMouseButtonControl.KeyboardAndMouse.Enums;
using YMouseButtonControl.KeyboardAndMouse.Interfaces;

namespace YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations.SimulatedKeystrokesTypes;

public class StickyHoldService : IStickyHoldService
{
    private readonly ISimulateKeyService _simulateKeyService;

    public StickyHoldService(ISimulateKeyService simulateKeyService)
    {
        _simulateKeyService = simulateKeyService;
    }

    public void StickyHold(IButtonMapping mapping, MouseButtonState state)
    {
        if (state != MouseButtonState.Pressed)
        {
            return;
        }

        if (mapping.State)
        {
            _simulateKeyService.ReleaseKeys(mapping.Keys);
            mapping.State = false;
        }
        else
        {
            _simulateKeyService.PressKeys(mapping.Keys);
            mapping.State = true;
        }
    }
}
