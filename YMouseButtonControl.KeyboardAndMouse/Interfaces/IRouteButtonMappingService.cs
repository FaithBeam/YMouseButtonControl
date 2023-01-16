using YMouseButtonControl.DataAccess.Models.Interfaces;
using YMouseButtonControl.KeyboardAndMouse.Enums;

namespace YMouseButtonControl.KeyboardAndMouse.Interfaces;

public interface IRouteButtonMappingService
{
    void Route(IButtonMapping mapping, MouseButtonState state);
}