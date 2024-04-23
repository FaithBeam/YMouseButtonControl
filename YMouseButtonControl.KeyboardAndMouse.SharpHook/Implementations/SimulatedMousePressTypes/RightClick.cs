using System;
using System.Threading;
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
                var t = new Thread(
                    () => eventSimulatorService.SimulateMousePress(YMouseButton.MouseButton2)
                );
                t.Start();
                break;
            case MouseButtonState.Released:
                t = new Thread(
                    () => eventSimulatorService.SimulateMouseRelease(YMouseButton.MouseButton2)
                );
                t.Start();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }
}
