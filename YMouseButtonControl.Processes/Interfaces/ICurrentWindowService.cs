using System;
using YMouseButtonControl.Services.Abstractions.Models.EventArgs;

namespace YMouseButtonControl.Processes.Interfaces;

public interface ICurrentWindowService
{
    event EventHandler<ActiveWindowChangedEventArgs> OnActiveWindowChangedEventHandler;
    void Dispose();
    public string ForegroundWindow { get; }
}