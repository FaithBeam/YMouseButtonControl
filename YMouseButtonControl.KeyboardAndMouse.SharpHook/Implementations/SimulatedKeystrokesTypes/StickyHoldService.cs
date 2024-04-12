using YMouseButtonControl.DataAccess.Models.Interfaces;
using YMouseButtonControl.KeyboardAndMouse.Enums;
using YMouseButtonControl.KeyboardAndMouse.Interfaces;

namespace YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations.SimulatedKeystrokesTypes;

public class StickyHoldService(ISimulateKeyService simulateKeyService) : IStickyHoldService
{
    public void StickyHold(IButtonMapping mapping, MouseButtonState state)
    {
        if (state != MouseButtonState.Pressed)
        {
            return;
        }

        if (mapping.State)
        {
            simulateKeyService.ReleaseKeys(mapping.Keys);
            mapping.State = false;
        }
        else
        {
            simulateKeyService.PressKeys(mapping.Keys);
            mapping.State = true;
        }
    }
}
