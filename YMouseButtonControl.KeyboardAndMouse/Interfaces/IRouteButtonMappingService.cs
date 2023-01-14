using YMouseButtonControl.DataAccess.Models.Interfaces;

namespace YMouseButtonControl.KeyboardAndMouse.Interfaces;

public interface IRouteButtonMappingService
{
    void Route(IButtonMapping mapping, bool pressed);
}