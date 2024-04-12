using System.Diagnostics;
using System.Linq;
using DynamicData;
using YMouseButtonControl.Core.Processes;
using YMouseButtonControl.Core.Services.Abstractions.Models;

namespace YMouseButtonControl.Services.MacOS;

public class MacOsProcessMonitorService : IProcessMonitorService
{
    public void Dispose() { }

    public IObservableCache<ProcessModel, int> RunningProcesses => GetProcesses();

    public bool ProcessRunning(string process) => Process.GetProcessesByName(process).Length != 0;

    private SourceCache<ProcessModel, int> GetProcesses()
    {
        var oc = new SourceCache<ProcessModel, int>(x => x.Process.Id);
        var procs = Process
            .GetProcesses()
            .Where(x => !string.IsNullOrWhiteSpace(x.ProcessName))
            .Select(x => new ProcessModel(x))
            .GroupBy(x => x.Process.ProcessName)
            .Select(x => x.First());
        oc.Edit(x => x.AddOrUpdate(procs));
        return oc;
    }
}