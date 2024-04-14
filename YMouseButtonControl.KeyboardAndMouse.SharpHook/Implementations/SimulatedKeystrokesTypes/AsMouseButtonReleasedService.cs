using YMouseButtonControl.Core.DataAccess.Models.Interfaces;
using YMouseButtonControl.Core.KeyboardAndMouse.Enums;
using YMouseButtonControl.Core.KeyboardAndMouse.Interfaces;

namespace YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations.SimulatedKeystrokesTypes;

public class AsMouseButtonReleasedService(IEventSimulatorService eventSimulatorService)
    : IAsMouseButtonReleasedService
{
    public void AsMouseButtonReleased(IButtonMapping mapping, MouseButtonState state)
    {
        if (state != MouseButtonState.Released)
        {
            return;
        }

        eventSimulatorService.TapKeys(mapping.Keys);
    }
}
