using YMouseButtonControl.Core.Services.KeyboardAndMouse.EventArgs;
using YMouseButtonControl.Core.ViewModels.Models;

namespace YMouseButtonControl.Core.Services.KeyboardAndMouse.Implementations.Queries.SkipProfile;

public interface ISkipProfile
{
    bool ShouldSkipProfile(ProfileVm p, NewMouseHookEventArgs e);
}
