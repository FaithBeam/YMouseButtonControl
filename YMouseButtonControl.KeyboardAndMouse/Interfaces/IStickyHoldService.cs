using YMouseButtonControl.DataAccess.Models.Interfaces;

namespace YMouseButtonControl.KeyboardAndMouse.Interfaces;

public interface IStickyHoldService
{
    void StickyHold(IButtonMapping mapping, bool pressed);
}