using YMouseButtonControl.Core.Processes;

namespace YMouseButtonControl.Services.Linux;

public class CurrentWindowService : ICurrentWindowService
{
    // Match every window. If someone figures out how to get the current window in X11 and/or wayland, make a PR please
    public string ForegroundWindow { get; } = "*";
}
