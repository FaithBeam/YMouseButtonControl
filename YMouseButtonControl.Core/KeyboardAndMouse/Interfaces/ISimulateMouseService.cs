using YMouseButtonControl.Core.DataAccess.Models.Enums;

namespace YMouseButtonControl.Core.KeyboardAndMouse.Interfaces;

public interface ISimulateMouseService
{
    void SimulateMousePress(YMouseButton mb);
    void SimulateMouseRelease(YMouseButton mb);
}
