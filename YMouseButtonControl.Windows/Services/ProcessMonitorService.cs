using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using YMouseButtonControl.Core.Services.Processes;

namespace YMouseButtonControl.Windows.Services;

[SupportedOSPlatform("windows5.1.2600")]
public class ProcessMonitorService : IProcessMonitorService
{
    public IEnumerable<ProcessModel> GetProcesses()
    {
        var cb = new ConcurrentBag<Process>();
        Parallel.ForEach(
            Process.GetProcesses().DistinctBy(x => x.ProcessName),
            p =>
            {
                try
                {
                    if (p.MainModule != null)
                    {
                        cb.Add(p);
                    }
                }
                catch (Win32Exception) { }
            }
        );
        return cb.DistinctBy(x => x.MainModule!.FileName)
            .Select(x => new ProcessModel(x)
            {
                Bitmap = GetBitmapStreamFromPath(x.MainModule!.FileName),
            })
            .ToList();
    }

    private static MemoryStream? GetBitmapStreamFromPath(string path)
    {
        if (path is null or "/")
        {
            return null;
        }

        var icon = Icon.ExtractAssociatedIcon(path);
        var bmp = icon?.ToBitmap();
        if (bmp is null)
        {
            return null;
        }
        var stream = new MemoryStream();
        bmp.Save(stream, ImageFormat.Bmp);
        stream.Position = 0;
        return stream;
    }
}
