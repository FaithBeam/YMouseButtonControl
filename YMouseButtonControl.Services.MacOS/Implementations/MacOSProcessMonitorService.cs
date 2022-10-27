using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using YMouseButtonControl.Processes.Interfaces;
using YMouseButtonControl.Services.Abstractions.Models;
using YMouseButtonControl.Services.Abstractions.Models.EventArgs;

namespace YMouseButtonControl.Services.MacOS.Implementations;

public class MacOSProcessMonitorService : IProcessMonitorService
{
    public void Dispose()
    {
        return;
    }

    public event EventHandler<ProcessChangedEventArgs> OnProcessChangedEventHandler;
    
    public IEnumerable<ProcessModel> GetProcesses()
    {
        return Process
            .GetProcesses()
            .Where(x => !string.IsNullOrWhiteSpace(x.ProcessName))
            .Select(x => new ProcessModel
            {
                ProcessId = (uint) x.Id,
                ProcessName = x.ProcessName,
                WindowTitle = x.MainWindowTitle
            })
            .OrderBy(x => x.ProcessName);
    }

    public bool ProcessRunning(string process)
    {
        return Process.GetProcessesByName(process).Any();
    }
}