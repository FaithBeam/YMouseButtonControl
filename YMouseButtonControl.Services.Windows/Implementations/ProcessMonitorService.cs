using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using YMouseButtonControl.Processes.Interfaces;
using YMouseButtonControl.Services.Abstractions.Models;

namespace YMouseButtonControl.Services.Windows.Implementations;

public class ProcessMonitorService : IProcessMonitorService
{
    private Dictionary<uint, string> _runningProcesses;
    private ManagementEventWatcher _createdEventWatcher;
    private ManagementEventWatcher _deletedEventWatcher;
    private readonly ManagementObjectSearcher _searcher = new ("SELECT * FROM WIN32_PROCESS");

    public ProcessMonitorService()
    {
        PopulateRunningProcesses();
        _createdEventWatcher = new ManagementEventWatcher("root\\CimV2", 
            "SELECT * FROM __InstanceCreationEvent WITHIN 1 WHERE TargetInstance ISA 'Win32_Process'");
        _createdEventWatcher.EventArrived += OnCreatedProcess;
        _createdEventWatcher.Start();
        _deletedEventWatcher = new ManagementEventWatcher("root\\CimV2", 
            "SELECT * FROM __InstanceDeletionEvent WITHIN 1 WHERE TargetInstance ISA 'Win32_Process'");
        _deletedEventWatcher.EventArrived += OnDeletedProcess;
        _deletedEventWatcher.Start();
    }

    private void OnDeletedProcess(object sender, EventArrivedEventArgs e)
    {
        var process = e.NewEvent.GetPropertyValue("TargetInstance") as ManagementBaseObject;
        _runningProcesses.TryAdd((uint)process.Properties["ProcessId"].Value, (string)process.Properties["Name"].Value);
    }

    private void OnCreatedProcess(object sender, EventArrivedEventArgs e)
    {
        var process = e.NewEvent.GetPropertyValue("TargetInstance") as ManagementBaseObject;
        _runningProcesses.Remove((uint)process.Properties["ProcessId"].Value);
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