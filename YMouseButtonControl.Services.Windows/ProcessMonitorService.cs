using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using YMouseButtonControl.Core.Processes;
using YMouseButtonControl.Core.Services.Abstractions.Models;

namespace YMouseButtonControl.Services.Windows;

[SupportedOSPlatform("windows5.1.2600")]
public class ProcessMonitorService : IProcessMonitorService
{
    public IEnumerable<ProcessModel> GetProcesses =>
        Process
            .GetProcesses()
            .Where(x =>
            {
                try
                {
                    return x.MainModule != null;
                }
                catch (Win32Exception)
                {
                    return false;
                }
            })
            .DistinctBy(x => x.MainModule!.FileName)
            .Select(x => new ProcessModel(x)
            {
                Bitmap = GetBitmapStreamFromPath(x.MainModule!.FileName)
            });

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
