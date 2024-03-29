using YMouseButtonControl.DataAccess.Models.Enums;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.KeyboardAndMouse.Enums;

namespace YMouseButtonControl.KeyboardAndMouse.Interfaces;

public interface IRouteMouseButtonService
{
    void Route(MouseButton b, Profile p, MouseButtonState state);
}
