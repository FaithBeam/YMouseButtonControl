using YMouseButtonControl.DataAccess.Models.Implementations;
using YMouseButtonControl.KeyboardAndMouse.Interfaces;
using YMouseButtonControl.Processes.Interfaces;

namespace YMouseButtonControl.KeyboardAndMouse.SharpHook.Implementations;

public class SkipProfileService : ISkipProfileService
{
    private readonly ICurrentWindowService _currentWindowService;

    public SkipProfileService(ICurrentWindowService currentWindowService)
    {
        _currentWindowService = currentWindowService;
    }

    // Returns whether or not this profile should be skipped on mouse events
    public bool ShouldSkipProfile(Profile p)
    {
        if (p.Process != "*" && !_currentWindowService.ForegroundWindow.Contains(p.Process))
        {
            return true;
        }

        // If the profile's checkbox is checked in the profiles list
        if (!p.Checked)
        {
            return true;
        }

        return false;
    }
}