using System;
using System.Collections.Generic;
using YMouseButtonControl.Services.Abstractions.Models;
using YMouseButtonControl.Services.Abstractions.Models.EventArgs;

namespace YMouseButtonControl.Processes.Interfaces;

public interface IProcessMonitorService
{
    event EventHandler<ProcessChangedEventArgs> OnProcessCreatedEventHandler;
    event EventHandler<ProcessChangedEventArgs> OnProcessDeletedEventHandler;

    void Dispose();
    IEnumerable<ProcessModel> GetProcesses();
    bool ProcessRunning(string process);
}