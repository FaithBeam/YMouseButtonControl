using YMouseButtonControl.Core.Services.KeyboardAndMouse.Enums;
using YMouseButtonControl.Core.ViewModels.Models;

namespace YMouseButtonControl.Core.Services.KeyboardAndMouse.Implementations.SimulatedKeystrokesTypes;

public interface IStickyHoldService
{
    void StickyHold(BaseButtonMappingVm mapping, MouseButtonState state);
}

public class StickyHoldService(IEventSimulatorService eventSimulatorService) : IStickyHoldService
{
    public void StickyHold(BaseButtonMappingVm mapping, MouseButtonState state)
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
