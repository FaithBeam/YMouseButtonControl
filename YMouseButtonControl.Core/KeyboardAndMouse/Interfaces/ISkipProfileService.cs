using YMouseButtonControl.Core.DataAccess.Models.Implementations;
using YMouseButtonControl.Core.Services.Abstractions.Models.EventArgs;

namespace YMouseButtonControl.Core.KeyboardAndMouse.Interfaces;

public interface ISkipProfileService
{
    bool ShouldSkipProfile(Profile p, NewMouseHookEventArgs e);
}
