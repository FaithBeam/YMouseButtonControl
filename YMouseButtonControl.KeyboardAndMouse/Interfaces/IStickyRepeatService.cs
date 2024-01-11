using YMouseButtonControl.DataAccess.Models.Interfaces;
using YMouseButtonControl.KeyboardAndMouse.Enums;

namespace YMouseButtonControl.KeyboardAndMouse.Interfaces;

public interface IStickyRepeatService
{
    void StickyRepeat(ISequencedMapping mapping, MouseButtonState state);
}