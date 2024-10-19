using Microsoft.Extensions.Logging;
using YMouseButtonControl.Core.Services.KeyboardAndMouse.EventArgs;
using YMouseButtonControl.Core.Services.KeyboardAndMouse.Interfaces;
using YMouseButtonControl.Core.Services.Processes;
using YMouseButtonControl.Core.ViewModels.Models;

namespace YMouseButtonControl.Linux.Services;

public partial class SkipProfileService(
    ILogger<SkipProfileService> logger,
    ICurrentWindowService currentWindowService
) : ISkipProfileService
{
    // Returns whether this profile should be skipped on mouse events
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

        if (currentWindowService.ForegroundWindow.Contains(p.Process))
        {
            return false;
        }

        if (currentWindowService.ForegroundWindow == "*")
        {
            return false;
        }

        LogForegroundWindow(logger, currentWindowService.ForegroundWindow);
        LogCouldNotFindForegroundWindow(logger, p.Process);
        return true;
    }

    [LoggerMessage(LogLevel.Information, "Couldn't find foreground window {Process}")]
    private static partial void LogCouldNotFindForegroundWindow(ILogger logger, string process);

    [LoggerMessage(LogLevel.Information, "Foreground window: {ForegroundWindow}")]
    private static partial void LogForegroundWindow(ILogger logger, string foregroundWindow);

    [LoggerMessage(LogLevel.Information, "Not checked")]
    private static partial void LogNotChecked(ILogger logger);
}
