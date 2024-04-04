using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.Services.Abstractions.Models.EventArgs;

namespace YMouseButtonControl.KeyboardAndMouse.Interfaces;

public interface ISkipProfileService
{
    bool ShouldSkipProfile(Profile p, NewMouseHookEventArgs e);
}
