using System.Diagnostics;
using System.IO;

namespace YMouseButtonControl.Core.ViewModels.ProcessSelectorDialogVm.Queries.Processes.Models;

public record ProcessModel(Process P)
{
    public Process Process { get; } = P;
    public Stream? Bitmap { get; set; }

    public void Dispose()
    {
        Process.Dispose();
        Bitmap?.Dispose();
    }
}
