using YMouseButtonControl.Core.DataAccess.Models.Interfaces;
using YMouseButtonControl.Core.KeyboardAndMouse.Enums;
using YMouseButtonControl.Core.KeyboardAndMouse.Interfaces;

namespace YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations.SimulatedKeystrokesTypes;

public class AsMouseButtonPressedService(IEventSimulatorService eventSimulatorService)
    : IAsMouseButtonPressedService
{
    public void AsMouseButtonPressed(IButtonMapping mapping, MouseButtonState state)
    {
        if (state != MouseButtonState.Pressed)
        {
            return;
        }

        eventSimulatorService.TapKeys(mapping.Keys);
    }
}
