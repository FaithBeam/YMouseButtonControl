using YMouseButtonControl.Core.Services.Processes;

namespace YMouseButtonControl.Linux.Services;

public class CurrentWindowService : ICurrentWindowService
{
    public string ForegroundWindow => "*";
}