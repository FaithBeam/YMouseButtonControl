using YMouseButtonControl.DataAccess.Models.Interfaces;
using YMouseButtonControl.KeyboardAndMouse.Enums;

namespace YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations.SimulatedKeystrokesTypes;

public interface IStickyRepeatService
{
    void StickyRepeat(IButtonMapping mapping, MouseButtonState state);
}
