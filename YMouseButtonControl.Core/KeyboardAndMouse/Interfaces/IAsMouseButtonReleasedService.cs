using YMouseButtonControl.Core.DataAccess.Models.Interfaces;
using YMouseButtonControl.Core.KeyboardAndMouse.Enums;

namespace YMouseButtonControl.Core.KeyboardAndMouse.Interfaces;

public interface IAsMouseButtonReleasedService
{
    void AsMouseButtonReleased(IButtonMapping mapping, MouseButtonState state);
}
