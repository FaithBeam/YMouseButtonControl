using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using YMouseButtonControl.Processes.Interfaces;
using YMouseButtonControl.Services.Abstractions.Models;

namespace YMouseButtonControl.Services.MacOS.Implementations;

public class MacOsProcessMonitorService : IProcessMonitorService
{
    public void Dispose()
    {
    }

    public ObservableCollection<ProcessModel> RunningProcesses => (ObservableCollection<ProcessModel>)GetProcesses();

    public bool ProcessRunning(string process)
    {
        return Process.GetProcessesByName(process).Any();
    }
    
    private IEnumerable<ProcessModel> GetProcesses()
    {
        return Process
            .GetProcesses()
            .Where(x => !string.IsNullOrWhiteSpace(x.ProcessName))
            .Select(x => new ProcessModel
            {
                ProcessId = (uint)x.Id,
                ProcessName = x.ProcessName.Trim(),
                WindowTitle = x.MainWindowTitle
            })
            .GroupBy(x => x.ProcessName)
            .Select(x => x.First());
    }
}