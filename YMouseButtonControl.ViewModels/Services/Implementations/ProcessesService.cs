using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using YMouseButtonControl.Services.Abstractions.Models;
using YMouseButtonControl.ViewModels.Services.Interfaces;

namespace YMouseButtonControl.ViewModels.Services.Implementations;

public class ProcessesService : IProcessesService
{
    public IEnumerable<ProcessModel> GetProcesses()
    {
        return Process.GetProcesses()
            .Where(x => x.MainWindowHandle != (IntPtr)0)
            .Select(x => new ProcessModel
            {
                ProcessName = x.ProcessName,
                WindowTitle = x.MainWindowTitle
            })
            .OrderBy(x => x.ProcessName);
    }
}