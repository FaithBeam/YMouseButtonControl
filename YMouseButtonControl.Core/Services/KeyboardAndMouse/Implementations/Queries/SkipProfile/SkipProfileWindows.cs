using System.Runtime.Versioning;
using Microsoft.Extensions.Logging;
using YMouseButtonControl.Core.Services.KeyboardAndMouse.EventArgs;
using YMouseButtonControl.Core.ViewModels.Models;

namespace YMouseButtonControl.Core.Services.KeyboardAndMouse.Implementations.Queries.SkipProfile;

[SupportedOSPlatform("windows5.1.2600")]
public partial class SkipProfileWindows(ILogger<SkipProfileWindows> logger) : ISkipProfile
{
    public bool ShouldSkipProfile(ProfileVm p, NewMouseHookEventArgs e)
    {
        // If the profile's checkbox is checked in the profiles list
        if (!p.Checked)
        {
            LogNotChecked(logger);
            return true;
        }

        if (p.Process == "*")
        {
            return false;
        }

        return !(e.ActiveWindow?.Contains(p.Process) ?? false);
    }

    [LoggerMessage(LogLevel.Information, "Not checked")]
    private static partial void LogNotChecked(ILogger logger);
}
