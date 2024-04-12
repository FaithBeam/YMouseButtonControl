using YMouseButtonControl.Core.DataAccess.Models.Interfaces;
using YMouseButtonControl.Core.KeyboardAndMouse.Enums;

namespace YMouseButtonControl.Core.KeyboardAndMouse.Interfaces;

public interface IAsMouseButtonPressedService
{
    void AsMouseButtonPressed(IButtonMapping mapping, MouseButtonState state);
}
