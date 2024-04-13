using System;
using System.Diagnostics;
using System.IO;
using ReactiveUI;

namespace YMouseButtonControl.Core.Services.Abstractions.Models;

public class ProcessModel(Process process) : ReactiveObject, IDisposable
{
    public Process Process { get; } = process;
    public Stream? Bitmap { get; set; }

    public void Dispose()
    {
        Process.Dispose();
        Bitmap?.Dispose();
    }
}
