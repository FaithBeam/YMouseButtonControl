using System.Runtime.Versioning;
using Serilog;
using YMouseButtonControl.Core.DataAccess.Models.Implementations;
using YMouseButtonControl.Core.KeyboardAndMouse.Interfaces;
using YMouseButtonControl.Core.Services.Abstractions.Models.EventArgs;

namespace YMouseButtonControl.Services.Windows.Implementations;

[SupportedOSPlatform("windows5.1.2600")]
public class SkipProfileService : ISkipProfileService
{
    private readonly ILogger _log = Log.Logger.ForContext<SkipProfileService>();

    public bool ShouldSkipProfile(Profile p, NewMouseHookEventArgs e)
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

        if (e.ActiveWindow?.Contains(p.Process) ?? false)
        {
            return false;
        }

        return true;
    }
}
