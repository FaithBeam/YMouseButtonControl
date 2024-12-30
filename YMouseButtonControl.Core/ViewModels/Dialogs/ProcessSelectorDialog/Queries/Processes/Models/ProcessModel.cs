using System.Diagnostics;
using System.IO;

namespace YMouseButtonControl.Core.ViewModels.Dialogs.ProcessSelectorDialog.Queries.Processes.Models;

public record ProcessModel(
    string WindowName,
    string ModuleName,
    string ProcessName,
    string Path,
    long Pid
)
{
    public Stream? Bitmap { get; set; }

    public void Dispose()
    {
        Bitmap?.Dispose();
    }
}
