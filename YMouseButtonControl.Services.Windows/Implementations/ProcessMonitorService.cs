using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using YMouseButtonControl.Processes.Interfaces;
using YMouseButtonControl.Services.Abstractions.Models;
using YMouseButtonControl.Services.Abstractions.Models.EventArgs;

namespace YMouseButtonControl.Services.Windows.Implementations;

public class ProcessMonitorService : IProcessMonitorService
{
    private Dictionary<uint, string> _runningProcesses;
    private readonly ManagementEventWatcher _createdEventWatcher;
    private readonly ManagementEventWatcher _deletedEventWatcher;
    private readonly ManagementObjectSearcher _searcher = new ("SELECT * FROM WIN32_PROCESS");

    public ProcessMonitorService()
    {
        PopulateRunningProcesses();
        _createdEventWatcher = new ManagementEventWatcher("root\\CimV2", "SELECT * FROM __InstanceCreationEvent WITHIN 1 WHERE TargetInstance ISA 'Win32_Process'");
        _createdEventWatcher.EventArrived += OnCreatedProcess;
        _createdEventWatcher.Start();
        _deletedEventWatcher = new ManagementEventWatcher("root\\CimV2", "SELECT * FROM __InstanceDeletionEvent WITHIN 1 WHERE TargetInstance ISA 'Win32_Process'");
        _deletedEventWatcher.EventArrived += OnDeletedProcess;
        _deletedEventWatcher.Start();
    }

    public event EventHandler<ProcessChangedEventArgs> OnProcessChangedEventHandler;

    private void OnProcessChanged(ProcessChangedEventArgs e)
    {
        var handler = OnProcessChangedEventHandler;
        handler?.Invoke(this, e);
    }
    
    private void OnDeletedProcess(object sender, EventArrivedEventArgs e)
    {
        var process = e.NewEvent.GetPropertyValue("TargetInstance") as ManagementBaseObject;
        var pm = new ProcessModel{ProcessId = (uint)process.Properties["ProcessId"].Value, ProcessName = (string)process.Properties["Name"].Value};
        _runningProcesses.Remove(pm.ProcessId);
        OnProcessChanged(new ProcessChangedEventArgs(pm));
    }

    private void OnCreatedProcess(object sender, EventArrivedEventArgs e)
    {
        var process = e.NewEvent.GetPropertyValue("TargetInstance") as ManagementBaseObject;
        var pm = new ProcessModel{ProcessId = (uint)process.Properties["ProcessId"].Value, ProcessName = (string)process.Properties["Name"].Value};
        _runningProcesses.TryAdd(pm.ProcessId, pm.ProcessName);
        OnProcessChanged(new ProcessChangedEventArgs(pm));
    }

    public IEnumerable<ProcessModel> GetProcesses()
    {
        return _runningProcesses.Select(x => new ProcessModel{ ProcessId = x.Key, ProcessName = x.Value });
    }

    public bool ProcessRunning(string process)
    {
        return _runningProcesses.ContainsValue(process);
    }
    
    private void PopulateRunningProcesses()
    {
        _runningProcesses = new Dictionary<uint, string>();
        foreach (var i in _searcher.Get())
        {
            _runningProcesses.TryAdd((uint)i.Properties["ProcessId"].Value, (string)i.Properties["Name"].Value);
        }
    }


    public void Dispose()
    {
        _createdEventWatcher.EventArrived -= OnCreatedProcess;
        _createdEventWatcher?.Dispose();
        _deletedEventWatcher.EventArrived -= OnDeletedProcess;
        _deletedEventWatcher?.Dispose();
    }
}