using SharpHook;
using SharpHook.Native;
using YMouseButtonControl.KeyboardAndMouse.Interfaces;

namespace YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations;

public class SimulateMouseService(IEventSimulator mouseSimulator) : ISimulateMouseService
{
    public void SimulateMousePress(MouseButton mb) => mouseSimulator.SimulateMousePress(mb);

    public void SimulateMouseRelease(MouseButton mb) => mouseSimulator.SimulateMouseRelease(mb);
}
