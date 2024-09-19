using System;
using YMouseButtonControl.Core.Services.KeyboardAndMouse.Enums;
using YMouseButtonControl.Core.Services.KeyboardAndMouse.Interfaces;
using YMouseButtonControl.Core.ViewModels.Models;

namespace YMouseButtonControl.Core.Services.KeyboardAndMouse.Implementations.SimulatedKeystrokesTypes;

public interface IDuringMousePressAndReleaseService
{
    void DuringMousePressAndRelease(BaseButtonMappingVm mapping, MouseButtonState state);
}

public class DuringMousePressAndReleaseService(IEventSimulatorService eventSimulatorService)
    : IDuringMousePressAndReleaseService
{
    public void DuringMousePressAndRelease(BaseButtonMappingVm mapping, MouseButtonState state)
    {
        switch (state)
        {
            case MouseButtonState.Pressed:
                eventSimulatorService.PressKeys(mapping.Keys);
                break;
            case MouseButtonState.Released:
                eventSimulatorService.ReleaseKeys(mapping.Keys);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }
}
