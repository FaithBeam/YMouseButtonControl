using YMouseButtonControl.DataAccess.Models.Interfaces;

namespace YMouseButtonControl.KeyboardAndMouse.Interfaces;

public interface IAsMouseButtonPressedService
{
    void AsMouseButtonPressed(IButtonMapping mapping, bool pressed);
}