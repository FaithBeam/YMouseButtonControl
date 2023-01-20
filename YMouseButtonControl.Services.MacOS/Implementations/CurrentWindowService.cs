using YMouseButtonControl.Processes.Interfaces;

namespace YMouseButtonControl.Services.MacOS.Implementations;

public class CurrentWindowService : ICurrentWindowService
{
    public void Run()
    {
    }

    public void Dispose()
    {
    }

    public string ForegroundWindow { get; private set; } = string.Empty;
}