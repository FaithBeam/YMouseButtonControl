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
    private Dictionary<uint, ProcessModel> _runningProcesses;
    private readonly ManagementEventWatcher _createdEventWatcher;
    private readonly ManagementEventWatcher _deletedEventWatcher;
    private readonly ManagementObjectSearcher _searcher = new("SELECT * FROM WIN32_PROCESS");
    private readonly object _processLockObject = new();
    private readonly WinApi _winApi = new();

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
            var pId = (uint)process.Properties["ProcessId"].Value;
            var pm = new ProcessModel
            {
                ProcessId = pId,
                ProcessName = (string)process.Properties["Name"].Value,
                WindowTitle = (string)process.Properties["Description"].Value,
                BitmapPath = _winApi.GetBitmapPathFromProcessId(pId)
            };
            if (_runningProcesses.Values.Any(x => x.ProcessName == pm.ProcessName))
            {
                return;
            }
            _runningProcesses.TryAdd(pm.ProcessId, pm);
            OnProcessChanged(new ProcessChangedEventArgs(pm));
        }
    }

    public IEnumerable<ProcessModel> GetProcesses()
    {
        lock (_processLockObject)
        {
            return _runningProcesses.Values.ToList();
        }
    }

    public bool ProcessRunning(string process)
    {
        lock (_processLockObject)
        {
            return _runningProcesses.Select(x => x.Value).Any(x => x.ProcessName == process);
        }
    }

    private void PopulateRunningProcesses()
    {
        lock (_processLockObject)
        {
            _runningProcesses = new Dictionary<uint, ProcessModel>();
            foreach (var i in _searcher.Get())
            {
                var pId = (uint)i.Properties["ProcessId"].Value;
                var hWnd = _winApi.GetHWndFromProcessId(pId);
                var pm = new ProcessModel()
                {
                    ProcessId = (uint)i.Properties["ProcessId"].Value,
                    ProcessName = (string)i.Properties["Name"].Value,
                    WindowTitle = (string)i.Properties["Description"].Value,
                    BitmapPath = _winApi.GetBitmapPathFromProcessId(pId)
                };
                if (_runningProcesses.Values.Any(x => x.ProcessName == pm.ProcessName))
                {
                    continue;
                }
                _runningProcesses.TryAdd(pm.ProcessId, pm);
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