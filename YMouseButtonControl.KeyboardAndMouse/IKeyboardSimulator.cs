using YMouseButtonControl.DataAccess.Models.Interfaces;

namespace YMouseButtonControl.KeyboardAndMouse;

public interface IKeyboardSimulator
{
    void SimulatedKeystrokes(IButtonMapping buttonMapping, bool pressed);
}