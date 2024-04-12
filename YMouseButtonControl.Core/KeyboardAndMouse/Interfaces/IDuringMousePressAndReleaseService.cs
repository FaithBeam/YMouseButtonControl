using YMouseButtonControl.Core.DataAccess.Models.Interfaces;
using YMouseButtonControl.Core.KeyboardAndMouse.Enums;

namespace YMouseButtonControl.Core.KeyboardAndMouse.Interfaces;

public interface IDuringMousePressAndReleaseService
{
    void DuringMousePressAndRelease(IButtonMapping mapping, MouseButtonState state);
}
