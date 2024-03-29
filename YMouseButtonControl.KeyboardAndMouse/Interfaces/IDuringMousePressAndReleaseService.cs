using YMouseButtonControl.DataAccess.Models.Interfaces;
using YMouseButtonControl.KeyboardAndMouse.Enums;

namespace YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations.SimulatedKeystrokesTypes;

public interface IDuringMousePressAndReleaseService
{
    void DuringMousePressAndRelease(IButtonMapping mapping, MouseButtonState state);
}
