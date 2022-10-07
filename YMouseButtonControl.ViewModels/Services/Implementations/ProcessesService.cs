﻿using System.Collections.Generic;
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
            .Where(x => !string.IsNullOrWhiteSpace(x.ProcessName))
            .Select(x => new ProcessModel
            {
                ProcessName = x.ProcessName,
                WindowTitle = string.IsNullOrWhiteSpace(x.MainWindowTitle) ? x.ProcessName : x.MainWindowTitle
            })
            .OrderBy(x => x.ProcessName);
    }
}