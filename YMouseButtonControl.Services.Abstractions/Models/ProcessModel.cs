using System.Diagnostics;
using ReactiveUI;

namespace YMouseButtonControl.Services.Abstractions.Models;

public class ProcessModel : ReactiveObject, IDisposable
{
    private bool _hasExited;

    public ProcessModel(Process process)
    {
        Process = process;
        Process.EnableRaisingEvents = true;
        Process.Exited += (_, _) => HasExited = true;
    }

    public Process Process { get; }
    public string? BitmapPath { get; set; }
    public bool HasExited
    {
        get => _hasExited;
        private set => this.RaiseAndSetIfChanged(ref _hasExited, value);
    }

    public void Dispose()
    {
        Process.Dispose();
    }
}
