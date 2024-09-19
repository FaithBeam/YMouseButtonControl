using YMouseButtonControl.Core.Services.KeyboardAndMouse.Enums;
using YMouseButtonControl.Core.Services.KeyboardAndMouse.Interfaces;
using YMouseButtonControl.Core.ViewModels.Models;

namespace YMouseButtonControl.Core.Services.KeyboardAndMouse.Implementations.SimulatedKeystrokesTypes;

public interface IAsMouseButtonPressedService
{
    void AsMouseButtonPressed(BaseButtonMappingVm mapping, MouseButtonState state);
}

public class AsMouseButtonPressedService(IEventSimulatorService eventSimulatorService)
    : IAsMouseButtonPressedService
{
    public void AsMouseButtonPressed(BaseButtonMappingVm mapping, MouseButtonState state)
    {
        if (state != MouseButtonState.Pressed)
        {
            return;
        }

        eventSimulatorService.TapKeys(mapping.Keys);
    }
}
