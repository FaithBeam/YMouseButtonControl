using YMouseButtonControl.Core.Services.KeyboardAndMouse.EventArgs;
using YMouseButtonControl.Core.ViewModels.Models;

namespace YMouseButtonControl.Core.Services.KeyboardAndMouse.Interfaces;

public interface ISkipProfileService
{
    bool ShouldSkipProfile(ProfileVm p, NewMouseHookEventArgs e);
}
