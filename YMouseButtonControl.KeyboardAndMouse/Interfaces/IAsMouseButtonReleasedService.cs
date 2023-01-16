using YMouseButtonControl.DataAccess.Models.Interfaces;
using YMouseButtonControl.KeyboardAndMouse.Enums;

namespace YMouseButtonControl.KeyboardAndMouse.Interfaces;

public interface IAsMouseButtonReleasedService
{
    void AsMouseButtonReleased(IButtonMapping mapping, MouseButtonState state);
}