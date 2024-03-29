using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DynamicData;
using YMouseButtonControl.Services.Abstractions.Models;
using YMouseButtonControl.Services.Abstractions.Models.EventArgs;

namespace YMouseButtonControl.Processes.Interfaces;

public interface IProcessMonitorService
{
    IObservableCache<ProcessModel, int> RunningProcesses { get; }
    bool ProcessRunning(string process);
    void Dispose();
}
