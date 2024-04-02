using System;
using SharpHook.Native;
using YMouseButtonControl.KeyboardAndMouse.Enums;
using YMouseButtonControl.KeyboardAndMouse.Interfaces;

namespace YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations.SimulatedMousePressTypes;

public class RightClick : IRightClick
{
    private readonly ISimulateMouseService _simulateMouseService;

    public RightClick(ISimulateMouseService simulateMouseService)
    {
        _simulateMouseService = simulateMouseService;
    }

    public void SimulateRightClick(MouseButtonState state)
    {
        switch (state)
        {
            case MouseButtonState.Pressed:
                _simulateMouseService.SimulateMousePress(MouseButton.Button2);
                break;
            case MouseButtonState.Released:
                _simulateMouseService.SimulateMouseRelease(MouseButton.Button2);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }
}
