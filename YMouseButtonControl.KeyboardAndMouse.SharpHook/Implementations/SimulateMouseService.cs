using SharpHook;
using SharpHook.Native;
using YMouseButtonControl.KeyboardAndMouse.Interfaces;

namespace YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations;

public class SimulateMouseService : ISimulateMouseService
{
    private readonly IEventSimulator _mouseSimulator;

    public SimulateMouseService(IEventSimulator mouseSimulator)
    {
        _mouseSimulator = mouseSimulator;
    }

    public void SimulateMousePress(MouseButton mb) => _mouseSimulator.SimulateMousePress(mb);

    public void SimulateMouseRelease(MouseButton mb) => _mouseSimulator.SimulateMouseRelease(mb);
}
