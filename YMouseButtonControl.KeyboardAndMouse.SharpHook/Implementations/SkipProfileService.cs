using System;
using Serilog;
using Serilog.Core;
using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.KeyboardAndMouse.Interfaces;
using YMouseButtonControl.Processes.Interfaces;

namespace YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations;

public class SkipProfileService : ISkipProfileService
{
    private readonly ICurrentWindowService _currentWindowService;
    private readonly ILogger _myLog;

    public SkipProfileService(ICurrentWindowService currentWindowService)
    {
        _currentWindowService = currentWindowService;
        _myLog = Log.Logger.ForContext<SkipProfileService>();
    }

    // Returns whether or not this profile should be skipped on mouse events
    public bool ShouldSkipProfile(Profile p)
    {
        if (p.Process != "*" && !_currentWindowService.ForegroundWindow.Contains(p.Process))
        {
            _myLog.Information(
                "Foreground window: {ForegroundWindow}",
                _currentWindowService.ForegroundWindow
            );
            _myLog.Information("Couldn't find foreground window {Process}", p.Process);
            return true;
        }

        // If the profile's checkbox is checked in the profiles list
        if (!p.Checked)
        {
            _myLog.Information("Not checked");
            return true;
        }

        return false;
    }
}
