using System;
using System.Threading;
using SharpHook.Native;
using YMouseButtonControl.Core.Services.KeyboardAndMouse.Enums;
using YMouseButtonControl.Core.Services.KeyboardAndMouse.Interfaces;

namespace YMouseButtonControl.Core.Services.KeyboardAndMouse.Implementations.SimulatedMousePressTypes;

public interface IRightClick
{
    void SimulateRightClick(MouseButtonState state);
}

public class RightClick(IEventSimulatorService eventSimulatorService) : IRightClick
{
    public void SimulateRightClick(MouseButtonState state)
    {
        switch (state)
        {
            case MouseButtonState.Pressed:
                var t = new Thread(
                    () => eventSimulatorService.SimulateMousePress(MouseButton.Button2)
                );
                t.Start();
                break;
            case MouseButtonState.Released:
                t = new Thread(
                    () => eventSimulatorService.SimulateMouseRelease(MouseButton.Button2)
                );
                t.Start();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }
}
