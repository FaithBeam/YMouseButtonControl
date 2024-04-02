using SharpHook.Native;

namespace YMouseButtonControl.KeyboardAndMouse.Interfaces;

public interface ISimulateMouseService
{
    void SimulateMousePress(MouseButton mb);
    void SimulateMouseRelease(MouseButton mb);
}
