using System;
using YMouseButtonControl.Core.DataAccess.Models.Interfaces;
using YMouseButtonControl.Core.KeyboardAndMouse.Enums;
using YMouseButtonControl.Core.KeyboardAndMouse.Interfaces;

namespace YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations.SimulatedKeystrokesTypes;

public class DuringMousePressAndReleaseService(IEventSimulatorService eventSimulatorService)
    : IDuringMousePressAndReleaseService
{
    public void DuringMousePressAndRelease(IButtonMapping mapping, MouseButtonState state)
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
