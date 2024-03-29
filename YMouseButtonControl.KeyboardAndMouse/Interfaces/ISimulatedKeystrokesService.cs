using YMouseButtonControl.DataAccess.Models.Interfaces;
using YMouseButtonControl.KeyboardAndMouse.Enums;

namespace YMouseButtonControl.KeyboardAndMouse.Interfaces;

public interface ISimulatedKeystrokesService
{
    void SimulatedKeystrokes(IButtonMapping buttonMapping, MouseButtonState state);
}
