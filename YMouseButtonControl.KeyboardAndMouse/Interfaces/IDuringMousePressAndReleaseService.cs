using YMouseButtonControl.DataAccess.Models.Interfaces;
using YMouseButtonControl.KeyboardAndMouse.Enums;

namespace YMouseButtonControl.KeyboardAndMouse.Interfaces;

public interface IDuringMousePressAndReleaseService
{
    void DuringMousePressAndRelease(ISequencedMapping mapping, MouseButtonState state);
}