using YMouseButtonControl.DataAccess.Models.Interfaces;
using YMouseButtonControl.KeyboardAndMouse.Enums;

namespace YMouseButtonControl.KeyboardAndMouse.Interfaces;

public interface IStickyHoldService
{
    void StickyHold(IButtonMapping mapping, MouseButtonState state);
}