using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using YMouseButtonControl.Core.Services.Processes;

namespace YMouseButtonControl.MacOS.Services;

public class ProcessMonitorService : IProcessMonitorService
{
    public IEnumerable<ProcessModel> GetProcesses() =>
        Process
            .GetProcesses()
            .Select(x => new ProcessModel(x))
            .Where(x => !string.IsNullOrWhiteSpace(x.Process.ProcessName));
}
