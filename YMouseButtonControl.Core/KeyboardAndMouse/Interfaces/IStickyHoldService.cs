using YMouseButtonControl.Core.DataAccess.Models.Interfaces;
using YMouseButtonControl.Core.KeyboardAndMouse.Enums;

namespace YMouseButtonControl.Core.KeyboardAndMouse.Interfaces;

public interface IStickyHoldService
{
    void StickyHold(IButtonMapping mapping, MouseButtonState state);
}
