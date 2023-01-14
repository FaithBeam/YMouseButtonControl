using YMouseButtonControl.DataAccess.Models.Interfaces;

namespace YMouseButtonControl.KeyboardAndMouse.Interfaces;

public interface ISimulatedKeystrokesService
{
    void SimulatedKeystrokes(IButtonMapping buttonMapping, bool pressed);
}