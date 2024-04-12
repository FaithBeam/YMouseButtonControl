using YMouseButtonControl.Core.DataAccess.Models.Interfaces;
using YMouseButtonControl.Core.KeyboardAndMouse.Enums;

namespace YMouseButtonControl.Core.KeyboardAndMouse.Interfaces;

public interface IStickyRepeatService
{
    void StickyRepeat(IButtonMapping mapping, MouseButtonState state);
}
