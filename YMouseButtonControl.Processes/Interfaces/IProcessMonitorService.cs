using System;
using System.Collections.Generic;
using YMouseButtonControl.Services.Abstractions.Models;

namespace YMouseButtonControl.Processes.Interfaces;

public interface IProcessMonitorService: IDisposable
{
    IEnumerable<ProcessModel> GetProcesses();
    bool ProcessRunning(string process);
}