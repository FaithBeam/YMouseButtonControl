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
    private Dictionary<uint, Tuple<string, string>> _runningProcesses;
    private readonly ManagementEventWatcher _createdEventWatcher;
    private readonly ManagementEventWatcher _deletedEventWatcher;
    private readonly ManagementObjectSearcher _searcher = new("SELECT * FROM WIN32_PROCESS");
    private readonly object _processLockObject = new();

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

    public event EventHandler<ProcessChangedEventArgs> OnProcessChangedEventHandler;

    private void OnProcessChanged(ProcessChangedEventArgs e)
    {
        var handler = OnProcessChangedEventHandler;
        handler?.Invoke(this, e);
    }

    private void OnDeletedProcess(object sender, EventArrivedEventArgs e)
    {
        lock (_processLockObject)
        {
            var process = e.NewEvent.GetPropertyValue("TargetInstance") as ManagementBaseObject;
            var pm = new ProcessModel
            {
                ProcessId = (uint)process.Properties["ProcessId"].Value,
                ProcessName = (string)process.Properties["Name"].Value
            };
            _runningProcesses.Remove(pm.ProcessId);
            OnProcessChanged(new ProcessChangedEventArgs(pm));            
        }
    }

    private void OnCreatedProcess(object sender, EventArrivedEventArgs e)
    {
        lock (_processLockObject)
        {
            var process = e.NewEvent.GetPropertyValue("TargetInstance") as ManagementBaseObject;
            var pm = new ProcessModel
            {
                ProcessId = (uint)process.Properties["ProcessId"].Value,
                ProcessName = (string)process.Properties["Name"].Value,
                WindowTitle = (string)process.Properties["Description"].Value
            };
            _runningProcesses.TryAdd(pm.ProcessId, new Tuple<string, string>(pm.ProcessName, ""));
            OnProcessChanged(new ProcessChangedEventArgs(pm));
        }
    }

    public IEnumerable<ProcessModel> GetProcesses()
    {
        lock (_processLockObject)
        {
            return _runningProcesses.Select(x => new ProcessModel { ProcessId = x.Key, ProcessName = x.Value.Item1, WindowTitle = x.Value.Item2});
        }
    }

    public bool ProcessRunning(string process)
    {
        lock (_processLockObject)
        {
            return _runningProcesses.Select(x => x.Value).Any(x => x.Item1 == process);
        }
    }

    private void PopulateRunningProcesses()
    {
        lock (_processLockObject)
        {
            _runningProcesses = new Dictionary<uint, Tuple<string, string>>();
            foreach (var i in _searcher.Get())
            {
                _runningProcesses.TryAdd((uint)i.Properties["ProcessId"].Value,
                    new Tuple<string, string>((string)i.Properties["Name"].Value, (string)i.Properties["Description"].Value));
            }       
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