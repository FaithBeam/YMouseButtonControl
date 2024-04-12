using YMouseButtonControl.Core.DataAccess.Models.Interfaces;
using YMouseButtonControl.Core.KeyboardAndMouse.Enums;
using YMouseButtonControl.Core.KeyboardAndMouse.Interfaces;

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
