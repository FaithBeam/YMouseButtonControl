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
            .Select(x => new ProcessModel
            {
                ProcessId = (uint) x.Id,
                ProcessName = x.ProcessName,
                WindowTitle = x.MainWindowTitle
            });
    }

    public bool ProcessRunning(string process)
    {
        return Process.GetProcessesByName(process).Any();
    }
}