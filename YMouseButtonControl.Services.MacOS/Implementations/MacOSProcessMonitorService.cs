using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using YMouseButtonControl.Processes.Interfaces;
using YMouseButtonControl.Services.Abstractions.Models;
using YMouseButtonControl.Services.Abstractions.Models.EventArgs;

namespace YMouseButtonControl.Services.MacOS.Implementations;

public class MacOsProcessMonitorService : IProcessMonitorService
{
    private readonly object _processLockObject = new();
    
    public void Dispose() {}

    public event EventHandler<ProcessChangedEventArgs> OnProcessCreatedEventHandler;
    public event EventHandler<ProcessChangedEventArgs> OnProcessDeletedEventHandler;

    public IEnumerable<ProcessModel> GetProcesses()
    {
        lock (_processLockObject)
        {
            return Process
                .GetProcesses()
                .Where(x => !string.IsNullOrWhiteSpace(x.ProcessName))
                .Select(x => new ProcessModel
                {
                    ProcessId = (uint) x.Id,
                    ProcessName = x.ProcessName.Trim(),
                    WindowTitle = x.MainWindowTitle
                })
                .GroupBy(x => x.ProcessName)
                .Select(x => x.First())
                .OrderBy(x => x.ProcessName);
        }
    }

    public bool ProcessRunning(string process)
    {
        lock (_processLockObject)
        {
            return Process.GetProcessesByName(process).Any();
        }
    }
}