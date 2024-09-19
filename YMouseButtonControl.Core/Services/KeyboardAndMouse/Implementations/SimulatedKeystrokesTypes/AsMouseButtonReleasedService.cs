using YMouseButtonControl.Core.Services.KeyboardAndMouse.Enums;
using YMouseButtonControl.Core.Services.KeyboardAndMouse.Interfaces;
using YMouseButtonControl.Core.ViewModels.Models;

namespace YMouseButtonControl.Core.Services.KeyboardAndMouse.Implementations.SimulatedKeystrokesTypes;

public interface IAsMouseButtonReleasedService
{
    void AsMouseButtonReleased(BaseButtonMappingVm mapping, MouseButtonState state);
}

public class AsMouseButtonReleasedService(IEventSimulatorService eventSimulatorService)
    : IAsMouseButtonReleasedService
{
    public void AsMouseButtonReleased(BaseButtonMappingVm mapping, MouseButtonState state)
    {
        if (state != MouseButtonState.Released)
        {
            return;
        }

        eventSimulatorService.TapKeys(mapping.Keys);
    }
}
