using YMouseButtonControl.DataAccess.Models.Implementations;

namespace YMouseButtonControl.KeyboardAndMouse.Interfaces;

public interface ISkipProfileService
{
    bool ShouldSkipProfile(Profile p);
}
