using YMouseButtonControl.DataAccess.Models.Interfaces;
using YMouseButtonControl.KeyboardAndMouse.Enums;

namespace YMouseButtonControl.KeyboardAndMouse.Interfaces;

public interface IAsMouseButtonReleasedService
{
    void AsMouseButtonReleased(ISequencedMapping mapping, MouseButtonState state);
}