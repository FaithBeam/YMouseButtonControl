using YMouseButtonControl.Core.DataAccess.Models.Interfaces;
using YMouseButtonControl.Core.KeyboardAndMouse.Enums;
using YMouseButtonControl.Core.KeyboardAndMouse.Interfaces;

namespace YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations.SimulatedKeystrokesTypes;

public class StickyHoldService(IEventSimulatorService eventSimulatorService) : IStickyHoldService
{
    public void StickyHold(IButtonMapping mapping, MouseButtonState state)
    {
        if (state != MouseButtonState.Pressed)
        {
            return;
        }

        if (mapping.State)
        {
            eventSimulatorService.ReleaseKeys(mapping.Keys);
            mapping.State = false;
        }
        else
        {
            eventSimulatorService.PressKeys(mapping.Keys);
            mapping.State = true;
        }
    }
}
