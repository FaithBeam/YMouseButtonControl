using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.Versioning;
using DynamicData;
using DynamicData.Binding;
using YMouseButtonControl.Processes.Interfaces;
using YMouseButtonControl.Services.Abstractions.Models;

namespace YMouseButtonControl.Services.Windows.Implementations;

[SupportedOSPlatform("windows5.1.2600")]
public class ProcessMonitorService : IProcessMonitorService, IDisposable
{
    private readonly object _lock = new();
    private readonly ManagementEventWatcher _createdEventWatcher;
    private readonly ManagementObjectSearcher _searcher;
    private readonly SourceCache<ProcessModel, int> _runningProcesses;
    private const string CreatedEventWatcherScope = "root\\CimV2";

    private const string CreatedEventWatcherQuery =
        "SELECT * FROM __InstanceCreationEvent WITHIN 1 WHERE TargetInstance ISA 'Win32_Process'";

    public ProcessMonitorService()
    {
        _searcher = new ManagementObjectSearcher("SELECT * FROM WIN32_PROCESS");
        _runningProcesses = new SourceCache<ProcessModel, int>(x => x.Process.Id);
        PopulateRunningProcesses();
        _createdEventWatcher = new ManagementEventWatcher(
            CreatedEventWatcherScope,
            CreatedEventWatcherQuery
        );
        _createdEventWatcher.EventArrived += OnCreatedProcess;
        _createdEventWatcher.Start();
        _runningProcesses
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
        _createdEventWatcher.Dispose();
    }

    private void OnCreatedProcess(object sender, EventArrivedEventArgs e)
    {
        if (e.NewEvent.GetPropertyValue("TargetInstance") is not ManagementBaseObject process)
        {
            return;
        }

        var processId = process.Properties["ProcessId"].Value.ToString();
        if (string.IsNullOrWhiteSpace(processId))
        {
            return;
        }

        var pId = int.Parse(processId);

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
            BitmapPath = GetBitmapFromPath(proc.MainModule?.FileName ?? string.Empty)
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
            var processId = i.Properties["ProcessId"].Value.ToString();
            if (string.IsNullOrWhiteSpace(processId))
            {
                return;
            }

            var pId = int.Parse(processId);

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
                continue;
            }

            var pm = new ProcessModel(proc)
            {
                BitmapPath = GetBitmapFromPath(proc.MainModule?.FileName ?? string.Empty)
            };

            lock (_lock)
            {
                if (
                    _runningProcesses.Items.Any(x =>
                        x.Process.ProcessName == pm.Process.ProcessName
                    )
                )
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
            _runningProcesses.Edit(x =>
            {
                if (propertyValue.Sender != null)
                    x.Remove(propertyValue.Sender);
            });
        }

        propertyValue.Sender.Dispose();
    }

    private static string GetBitmapFromPath(string path)
    {
        if (path is null or "/")
        {
            return string.Empty;
        }

        var destination = Path.Join("cache", Path.GetFileName(path + ".ico"));
        if (File.Exists(destination))
        {
            return destination;
        }

        if (!Directory.Exists("cache"))
        {
            Directory.CreateDirectory("cache");
        }

        var icon = Icon.ExtractAssociatedIcon(path);
        if (icon is null)
        {
            return string.Empty;
        }

        var bmp = icon.ToBitmap();
        bmp.Save(destination);

        return destination;
    }
}
