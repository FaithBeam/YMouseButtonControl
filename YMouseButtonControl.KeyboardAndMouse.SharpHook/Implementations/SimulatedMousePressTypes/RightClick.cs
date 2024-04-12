using System;
using SharpHook.Native;
using YMouseButtonControl.KeyboardAndMouse.Enums;
using YMouseButtonControl.KeyboardAndMouse.Interfaces;

namespace YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations.SimulatedMousePressTypes;

public class RightClick(ISimulateMouseService simulateMouseService) : IRightClick
{
    public void SimulateRightClick(MouseButtonState state)
    {
        switch (state)
        {
            case MouseButtonState.Pressed:
                simulateMouseService.SimulateMousePress(MouseButton.Button2);
                break;
            case MouseButtonState.Released:
                simulateMouseService.SimulateMouseRelease(MouseButton.Button2);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }
}
