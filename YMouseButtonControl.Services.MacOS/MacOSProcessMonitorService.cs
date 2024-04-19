using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using YMouseButtonControl.Core.Processes;
using YMouseButtonControl.Core.Services.Abstractions.Models;

namespace YMouseButtonControl.Services.MacOS;

public class MacOsProcessMonitorService : IProcessMonitorService
{
    public IEnumerable<ProcessModel> GetProcesses() =>
        Process
            .GetProcesses()
            .Select(x => new ProcessModel(x))
            .Where(x => !string.IsNullOrWhiteSpace(x.Process.ProcessName));
}
