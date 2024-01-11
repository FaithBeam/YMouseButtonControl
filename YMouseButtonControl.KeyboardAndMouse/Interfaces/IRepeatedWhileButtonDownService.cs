using YMouseButtonControl.DataAccess.Models.Interfaces;
using YMouseButtonControl.KeyboardAndMouse.Enums;

namespace YMouseButtonControl.KeyboardAndMouse.Interfaces;

public interface IRepeatedWhileButtonDownService
{
    void RepeatWhileDown(ISequencedMapping mapping, MouseButtonState state);
}