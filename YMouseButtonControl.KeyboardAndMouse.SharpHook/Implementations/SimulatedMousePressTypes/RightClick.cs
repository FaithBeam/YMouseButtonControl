using System;
using YMouseButtonControl.Core.DataAccess.Models.Enums;
using YMouseButtonControl.Core.KeyboardAndMouse.Enums;
using YMouseButtonControl.Core.KeyboardAndMouse.Interfaces;

namespace YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations.SimulatedMousePressTypes;

public class RightClick(ISimulateMouseService simulateMouseService) : IRightClick
{
    public void SimulateRightClick(MouseButtonState state)
    {
        switch (state)
        {
            case MouseButtonState.Pressed:
                simulateMouseService.SimulateMousePress(YMouseButton.MouseButton2);
                break;
            case MouseButtonState.Released:
                simulateMouseService.SimulateMouseRelease(YMouseButton.MouseButton2);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }
}
