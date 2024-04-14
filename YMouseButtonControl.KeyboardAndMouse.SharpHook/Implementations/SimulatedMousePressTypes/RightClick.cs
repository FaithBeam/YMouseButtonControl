using System;
using YMouseButtonControl.Core.DataAccess.Models.Enums;
using YMouseButtonControl.Core.KeyboardAndMouse.Enums;
using YMouseButtonControl.Core.KeyboardAndMouse.Interfaces;

namespace YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations.SimulatedMousePressTypes;

public class RightClick(IEventSimulatorService eventSimulatorService) : IRightClick
{
    public void SimulateRightClick(MouseButtonState state)
    {
        switch (state)
        {
            case MouseButtonState.Pressed:
                eventSimulatorService.SimulateMousePress(YMouseButton.MouseButton2);
                break;
            case MouseButtonState.Released:
                eventSimulatorService.SimulateMouseRelease(YMouseButton.MouseButton2);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }
}
