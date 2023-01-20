using System;
using System.Collections.Generic;
using System.Linq;
using YMouseButtonControl.Processes.Interfaces;
using YMouseButtonControl.Profiles.Interfaces;
using YMouseButtonControl.Services.Abstractions.Models.EventArgs;

namespace YMouseButtonControl.Services.Windows.Implementations;

public class PayloadInjectorService : IDisposable, IPayloadInjectorService
{
    private IProfilesService _profilesService;
    private IProcessMonitorService _processMonitorService;
    private Dictionary<uint, Payload> _activePayloads = new();

    public PayloadInjectorService(IProfilesService profilesService, IProcessMonitorService processMonitorService)
    {
        _profilesService = profilesService;
        _processMonitorService = processMonitorService;
        // _processMonitorService.OnProcessCreatedEventHandler += OnProcessCreated;
        // _processMonitorService.OnProcessDeletedEventHandler += OnProcessDeleted;
    }

    public void Dispose()
    {
        // _processMonitorService.OnProcessCreatedEventHandler -= OnProcessCreated;
        // _processMonitorService.OnProcessDeletedEventHandler -= OnProcessDeleted;
    }

    // private void OnProcessCreated(object sender, ProcessChangedEventArgs e)
    // {
    //     foreach (var p in _profilesService.Profiles)
    //     {
    //         if (p.Process == e.ProcessModel.ProcessName)
    //         {
    //             var payload = new Payload(e.ProcessModel.ProcessId, p);
    //             if (_activePayloads.TryAdd(e.ProcessModel.ProcessId, payload))
    //             {
    //                 payload.Run();
    //             }
    //             else
    //             {
    //                 throw new Exception($"Couldn't add new payload: {e.ProcessModel.ProcessName} {e.ProcessModel.ProcessId}");
    //             }
    //         }
    //     }
    // }
    //
    // private void OnProcessDeleted(object sender, ProcessChangedEventArgs e)
    // {
    //     if (_activePayloads.TryGetValue(e.ProcessModel.ProcessId, out var payload))
    //     {
    //         payload.Dispose();
    //         if (!_activePayloads.Remove(e.ProcessModel.ProcessId))
    //         {
    //             throw new Exception($"Error removing {e.ProcessModel.ProcessId} from active payloads");
    //         }
    //     }
    // }
}
