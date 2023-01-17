using System;
using YMouseButtonControl.Processes.Interfaces;
using YMouseButtonControl.Services.Abstractions.Models.EventArgs;

namespace YMouseButtonControl.Services.MacOS.Implementations;

public class CurrentWindowService : ICurrentWindowService
{
    public event EventHandler<ActiveWindowChangedEventArgs> OnActiveWindowChangedEventHandler;
    public void Run()
    {
    }

    public void Dispose()
    {
    }

    public string ForegroundWindow { get; private set; } = string.Empty;
}