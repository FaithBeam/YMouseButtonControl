using YMouseButtonControl.DataAccess.Models.Interfaces;
using YMouseButtonControl.KeyboardAndMouse.Enums;

namespace YMouseButtonControl.KeyboardAndMouse.Interfaces;

public interface ISimulatedKeystrokesService
{
    void SimulatedKeystrokes(ISequencedMapping buttonMapping, MouseButtonState state);
}