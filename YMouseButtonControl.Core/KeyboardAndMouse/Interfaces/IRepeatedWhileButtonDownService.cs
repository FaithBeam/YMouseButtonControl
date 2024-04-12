using YMouseButtonControl.Core.DataAccess.Models.Interfaces;
using YMouseButtonControl.Core.KeyboardAndMouse.Enums;

namespace YMouseButtonControl.Core.KeyboardAndMouse.Interfaces;

public interface IRepeatedWhileButtonDownService
{
    void RepeatWhileDown(IButtonMapping mapping, MouseButtonState state);
}
