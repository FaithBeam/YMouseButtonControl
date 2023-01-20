using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using YMouseButtonControl.Processes.Interfaces;
using YMouseButtonControl.Services.Abstractions.Models;
using YMouseButtonControl.Services.Abstractions.Models.EventArgs;
using YMouseButtonControl.Services.Windows.Implementations.Win32Stuff;

namespace YMouseButtonControl.Services.Windows.Implementations;

public class ProcessMonitorService : IProcessMonitorService
{
    private List<ProcessModel> _runningProcesses;
    private readonly object _lock = new();
    private readonly ManagementEventWatcher _createdEventWatcher;
    private readonly ManagementEventWatcher _deletedEventWatcher;
    private readonly ManagementObjectSearcher _searcher = new("SELECT * FROM WIN32_PROCESS");
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

    public event EventHandler<ProcessChangedEventArgs> OnProcessCreatedEventHandler;
    public event EventHandler<ProcessChangedEventArgs> OnProcessDeletedEventHandler;

    public IEnumerable<ProcessModel> GetProcesses()
    {
        lock (_lock)
        {
            return _runningProcesses;
        }
    }

    public bool ProcessRunning(string process)
    {
        lock (_lock)
        {
            return _runningProcesses.Any(x => x.ProcessName == process);
        }
    }
    
    public void Dispose()
    {
        _createdEventWatcher.EventArrived -= OnCreatedProcess;
        _createdEventWatcher?.Dispose();
        _deletedEventWatcher.EventArrived -= OnDeletedProcess;
        _deletedEventWatcher?.Dispose();
    }
    
    private void OnProcessCreated(ProcessChangedEventArgs e)
    {
        var handler = OnProcessCreatedEventHandler;
        handler?.Invoke(this, e);
    }

    private void OnProcessDeleted(ProcessChangedEventArgs e)
    {
        var handler = OnProcessDeletedEventHandler;
        handler?.Invoke(this, e);
    }

    private void OnDeletedProcess(object sender, EventArrivedEventArgs e)
    {
        var process = e.NewEvent.GetPropertyValue("TargetInstance") as ManagementBaseObject;
        var pm = new ProcessModel
        {
            ProcessId = (uint)process.Properties["ProcessId"].Value,
            ProcessName = (string)process.Properties["Name"].Value
        };

        lock (_lock)
        {
            _runningProcesses.RemoveAll(x => x.ProcessName == pm.ProcessName);
        }
        OnProcessDeleted(new ProcessChangedEventArgs(pm));
    }

    private void OnCreatedProcess(object sender, EventArrivedEventArgs e)
    {
        var process = e.NewEvent.GetPropertyValue("TargetInstance") as ManagementBaseObject;
        var pId = (uint)process.Properties["ProcessId"].Value;
        pId = EnumWindowsService.GetPidAssociatedWithWindowFromPid(pId);
        var hWnd = EnumWindowsService.GetHWndFromProcessId(pId);
        if (!_winApi.GetWindowTitleFromHwnd(hWnd, out var windowTitle))
        {
            windowTitle = (string)process.Properties["Description"].Value;
        }

        var pm = new ProcessModel
        {
            ProcessId = pId,
            ProcessName = (string)process.Properties["Name"].Value,
            WindowTitle = windowTitle,
            BitmapPath = _winApi.GetBitmapPathFromProcessId(pId)
        };

        lock (_lock)
        {
            if (_runningProcesses.Any(x => x.ProcessName == pm.ProcessName))
            {
                return;
            }

            _runningProcesses.Add(pm);
        }

        OnProcessCreated(new ProcessChangedEventArgs(pm));
    }

    private void PopulateRunningProcesses()
    {
        _runningProcesses = new List<ProcessModel>();

        foreach (var i in _searcher.Get())
        {
            var pId = (uint)i.Properties["ProcessId"].Value;
            pId = EnumWindowsService.GetPidAssociatedWithWindowFromPid(pId);

            var hWnd = EnumWindowsService.GetHWndFromProcessId(pId);

            if (!_winApi.GetWindowTitleFromHwnd(hWnd, out var windowTitle))
            {
                windowTitle = (string)i.Properties["Description"].Value;
            }

            var pm = new ProcessModel
            {
                ProcessId = pId,
                ProcessName = (string)i.Properties["Name"].Value,
                WindowTitle = windowTitle,
                BitmapPath = _winApi.GetBitmapPathFromProcessId(pId)
            };

            if (_runningProcesses.Any(x => x.ProcessName == pm.ProcessName))
            {
                continue;
            }

            _runningProcesses.Add(pm);
        }
    }
}