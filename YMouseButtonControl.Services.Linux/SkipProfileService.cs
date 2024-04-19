using Serilog;
using YMouseButtonControl.Core.DataAccess.Models.Implementations;
using YMouseButtonControl.Core.KeyboardAndMouse.Interfaces;
using YMouseButtonControl.Core.Processes;
using YMouseButtonControl.Core.Services.Abstractions.Models.EventArgs;

namespace YMouseButtonControl.Services.Linux;

public class SkipProfileService(ICurrentWindowService currentWindowService) : ISkipProfileService
{
    private readonly ILogger _myLog = Log.Logger.ForContext<SkipProfileService>();

    // Returns whether this profile should be skipped on mouse events
    public bool ShouldSkipProfile(Profile p, NewMouseHookEventArgs e)
    {
        // If the profile's checkbox is checked in the profiles list
        if (!p.Checked)
        {
            _myLog.Information("Not checked");
            return true;
        }

        if (p.Process == "*")
        {
            return false;
        }

        if (currentWindowService.ForegroundWindow.Contains(p.Process))
        {
            return false;
        }

        if (currentWindowService.ForegroundWindow == "*")
        {
            return false;
        }

        _myLog.Information(
            "Foreground window: {ForegroundWindow}",
            currentWindowService.ForegroundWindow
        );
        _myLog.Information("Couldn't find foreground window {Process}", p.Process);
        return true;
    }
}
