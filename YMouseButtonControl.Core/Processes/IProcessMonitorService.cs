using System;
using DynamicData;
using YMouseButtonControl.Core.Services.Abstractions.Models;

namespace YMouseButtonControl.Core.Processes;

public interface IProcessMonitorService : IDisposable
{
    IObservableCache<ProcessModel, int> RunningProcesses { get; }
}
