using YMouseButtonControl.Core.Processes;

namespace YMouseButtonControl.Services.MacOS;

public class CurrentWindowService : ICurrentWindowService
{
    public string ForegroundWindow { get; private set; } = string.Empty;
}
