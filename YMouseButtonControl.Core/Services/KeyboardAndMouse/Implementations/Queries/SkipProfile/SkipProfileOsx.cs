using Microsoft.Extensions.Logging;
using YMouseButtonControl.Core.Services.KeyboardAndMouse.EventArgs;
using YMouseButtonControl.Core.Services.KeyboardAndMouse.Implementations.Queries.CurrentWindow;
using YMouseButtonControl.Core.Services.KeyboardAndMouse.Implementations.Queries.SkipProfile;
using YMouseButtonControl.Core.ViewModels.Models;

namespace YMouseButtonControl.MacOS.Services;

public partial class SkipProfileOsx(ILogger<SkipProfileOsx> logger, IGetCurrentWindow currentWindow)
    : ISkipProfile
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

        if (currentWindow.ForegroundWindow.Contains(p.Process))
        {
            return false;
        }

        LogForegroundWindow(logger, currentWindow.ForegroundWindow);
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
