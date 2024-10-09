using System.Diagnostics;
using YMouseButtonControl.Core.Services.Processes;

namespace YMouseButtonControl.Linux.Services;

public class ProcessMonitorService : IProcessMonitorService
{
    public IEnumerable<ProcessModel> GetProcesses() =>
        Process
            .GetProcesses()
            .Select(x => new ProcessModel(x))
            .Where(x => !string.IsNullOrWhiteSpace(x.Process.MainModule?.ModuleName) && !string.IsNullOrWhiteSpace(x.Process.ProcessName));
}
