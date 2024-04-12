using SharpHook;
using SharpHook.Native;
using YMouseButtonControl.Core.DataAccess.Models.Enums;
using YMouseButtonControl.Core.KeyboardAndMouse.Interfaces;

namespace YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations;

public class SimulateMouseService(IEventSimulator mouseSimulator) : ISimulateMouseService
{
    public void SimulateMousePress(YMouseButton mb) =>
        mouseSimulator.SimulateMousePress((MouseButton)mb);

    public void SimulateMouseRelease(YMouseButton mb) =>
        mouseSimulator.SimulateMouseRelease((MouseButton)mb);
}
