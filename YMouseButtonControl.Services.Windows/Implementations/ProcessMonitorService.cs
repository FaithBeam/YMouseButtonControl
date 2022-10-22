using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using YMouseButtonControl.Processes.Interfaces;
using YMouseButtonControl.Services.Abstractions.Models;

namespace YMouseButtonControl.Services.Windows.Implementations;

public class ProcessMonitorService : IProcessMonitorService
{
    public IEnumerable<ProcessModel> GetProcesses()
    {
        return Process.GetProcesses()
            .Where(x => !string.IsNullOrWhiteSpace(x.ProcessName))
            .Select(x => new ProcessModel
            {
                ProcessName = x.ProcessName,
                WindowTitle = string.IsNullOrWhiteSpace(x.MainWindowTitle) ? x.ProcessName : x.MainWindowTitle
            })
            .OrderBy(x => x.ProcessName);
    }
}