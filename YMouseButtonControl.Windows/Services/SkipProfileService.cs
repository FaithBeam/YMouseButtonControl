using System.Runtime.Versioning;
using Serilog;
using YMouseButtonControl.Core.Services.KeyboardAndMouse.EventArgs;
using YMouseButtonControl.Core.Services.KeyboardAndMouse.Interfaces;
using YMouseButtonControl.Core.ViewModels.Models;

namespace YMouseButtonControl.Windows.Services;

[SupportedOSPlatform("windows5.1.2600")]
public class SkipProfileService : ISkipProfileService
{
    private readonly ILogger _log = Log.Logger.ForContext<SkipProfileService>();

    public bool ShouldSkipProfile(ProfileVm p, NewMouseHookEventArgs e)
    {
        // If the profile's checkbox is checked in the profiles list
        if (!p.Checked)
        {
            _log.Information("Not checked");
            return true;
        }

        if (p.Process == "*")
        {
            return false;
        }

        return !(e.ActiveWindow?.Contains(p.Process) ?? false);
    }
}
