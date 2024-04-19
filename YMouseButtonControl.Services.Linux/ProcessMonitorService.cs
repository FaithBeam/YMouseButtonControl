using System.Diagnostics;
using YMouseButtonControl.Core.Processes;
using YMouseButtonControl.Core.Services.Abstractions.Models;

namespace YMouseButtonControl.Services.Linux;

public class ProcessMonitorService : IProcessMonitorService
{
    public IEnumerable<ProcessModel> GetProcesses() =>
        Process
            .GetProcesses()
            .Select(x => new ProcessModel(x))
            .Where(x => !string.IsNullOrWhiteSpace(x.Process.ProcessName));
}
