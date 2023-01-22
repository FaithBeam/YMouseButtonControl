using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Management;
using DynamicData;
using DynamicData.Binding;
using YMouseButtonControl.Processes.Interfaces;
using YMouseButtonControl.Services.Abstractions.Models;
using YMouseButtonControl.Services.Windows.Implementations.Win32Stuff;

namespace YMouseButtonControl.Services.Windows.Implementations;

public class ProcessMonitorService : IProcessMonitorService
{
    private readonly object _lock = new();
    private readonly ManagementEventWatcher _createdEventWatcher;
    private readonly ManagementObjectSearcher _searcher = new("SELECT * FROM WIN32_PROCESS");
    private readonly WinApi _winApi = new();
    private readonly SourceCache<ProcessModel, int> _runningProcesses;
    private const string CreatedEventWatcherScope = "root\\CimV2";
    private const string CreatedEventWatcherQuery = "SELECT * FROM __InstanceCreationEvent WITHIN 1 WHERE TargetInstance ISA 'Win32_Process'";

    public ProcessMonitorService()
    {
        _runningProcesses = new SourceCache<ProcessModel, int>(x => x.Process.Id);
        PopulateRunningProcesses();
        _createdEventWatcher = new ManagementEventWatcher(CreatedEventWatcherScope, CreatedEventWatcherQuery);
        _createdEventWatcher.EventArrived += OnCreatedProcess;
        _createdEventWatcher.Start();
        var someBs = _runningProcesses
            .Connect()
            .WhenPropertyChanged(x => x.HasExited)
            .Subscribe(OnProcessHasExited);
    }

    public IObservableCache<ProcessModel, int> RunningProcesses
    {
        get
        {
            lock (_lock)
            {
                return _runningProcesses.AsObservableCache();
            }
        }
    }

    public bool ProcessRunning(string process)
    {
        lock (_lock)
        {
            return _runningProcesses.Items.Any(x => x.Process.ProcessName == process);
        }
    }

    public void Dispose()
    {
        _createdEventWatcher.EventArrived -= OnCreatedProcess;
        _createdEventWatcher?.Dispose();
    }

    private void OnCreatedProcess(object sender, EventArrivedEventArgs e)
    {
        var process = e.NewEvent.GetPropertyValue("TargetInstance") as ManagementBaseObject;

        var pId = int.Parse(process.Properties["ProcessId"].Value.ToString());

        Process proc;
        // Process may have already exited
        try
        {
            proc = Process.GetProcessById(pId);
        }
        catch (ArgumentException)
        {
            return;
        }

        // Skip processes we have no access to. Better way than this?
        try
        {
            _ = proc.SafeHandle;
        }
        catch (Win32Exception)
        {
            return;
        }

        var pm = new ProcessModel(proc)
        {
            BitmapPath = _winApi.GetBitmapFromPath(proc.MainModule?.FileName) ?? string.Empty
        };

        lock (_lock)
        {
            if (RunningProcesses.Items.Any(x => x.Process.ProcessName == pm.Process.ProcessName))
            {
                return;
            }

            _runningProcesses.Edit(x => x.AddOrUpdate(pm));
        }
    }

    private void PopulateRunningProcesses()
    {
        foreach (var i in _searcher.Get())
        {
            var pId = int.Parse(i.Properties["ProcessId"].Value.ToString());

            Process proc;
            // Process may have already exited
            try
            {
                proc = Process.GetProcessById(pId);
            }
            catch (ArgumentException)
            {
                return;
            }

            // Skip processes we have no access to. Better way than this?
            try
            {
                _ = proc.SafeHandle;
            }
            catch (Win32Exception e)
            {
                continue;
            }

            var pm = new ProcessModel(proc)
            {
                BitmapPath = _winApi.GetBitmapFromPath(proc.MainModule?.FileName) ?? string.Empty
            };

            lock (_lock)
            {
                if (_runningProcesses.Items.Any(x => x.Process.ProcessName == pm.Process.ProcessName))
                {
                    continue;
                }

                _runningProcesses.Edit(x => x.AddOrUpdate(pm));
            }
        }
    }

    private void OnProcessHasExited(PropertyValue<ProcessModel, bool> propertyValue)
    {
        if (!propertyValue.Value)
        {
            return;
        }

        lock (_lock)
        {
            _runningProcesses.Edit(x => x.Remove(propertyValue.Sender));
        }
        propertyValue.Sender.Dispose();
    }
}