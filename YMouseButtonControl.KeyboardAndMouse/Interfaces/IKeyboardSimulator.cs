using YMouseButtonControl.DataAccess.Models.Interfaces;

namespace YMouseButtonControl.KeyboardAndMouse.Interfaces;

public interface IKeyboardSimulator
{
    void SimulatedKeystrokes(IButtonMapping buttonMapping, bool pressed);
}