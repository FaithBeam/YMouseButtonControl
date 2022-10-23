using System;
using System.Collections.Generic;
using YMouseButtonControl.Services.Abstractions.Models;
using YMouseButtonControl.Services.Abstractions.Models.EventArgs;

namespace YMouseButtonControl.Processes.Interfaces;

public interface IProcessMonitorService : IDisposable
{
    event EventHandler<ProcessChangedEventArgs> OnProcessChangedEventHandler;
    IEnumerable<ProcessModel> GetProcesses();
    bool ProcessRunning(string process);
}