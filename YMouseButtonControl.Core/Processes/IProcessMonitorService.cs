using DynamicData;
using YMouseButtonControl.Core.Services.Abstractions.Models;

namespace YMouseButtonControl.Core.Processes;

public interface IProcessMonitorService
{
    IObservableCache<ProcessModel, int> RunningProcesses { get; }
    bool ProcessRunning(string process);
    void Dispose();
}
