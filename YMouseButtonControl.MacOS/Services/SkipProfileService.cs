using Serilog;
using YMouseButtonControl.Core.Services.KeyboardAndMouse.EventArgs;
using YMouseButtonControl.Core.Services.KeyboardAndMouse.Interfaces;
using YMouseButtonControl.Core.Services.Processes;
using YMouseButtonControl.Core.ViewModels.Models;

namespace YMouseButtonControl.MacOS.Services;

public class SkipProfileService(ICurrentWindowService currentWindowService) : ISkipProfileService
{
    private readonly ILogger _myLog = Log.Logger.ForContext<SkipProfileService>();

    // Returns whether this profile should be skipped on mouse events
    public bool ShouldSkipProfile(ProfileVm p, NewMouseHookEventArgs e)
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

        _myLog.Information(
            "Foreground window: {ForegroundWindow}",
            currentWindowService.ForegroundWindow
        );
        _myLog.Information("Couldn't find foreground window {Process}", p.Process);
        return true;
    }
}
