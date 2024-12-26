using System;
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
using YMouseButtonControl.Core.ViewModels.Dialogs.ProcessSelectorDialog.Queries.Processes.Models;

namespace YMouseButtonControl.Core.ViewModels.Dialogs.ProcessSelectorDialog.Queries.Processes;

[SupportedOSPlatform("windows5.1.2600")]
public static class ListProcessesWindows
{
    public sealed class Handler : IListProcessesHandler
    {
        public IEnumerable<ProcessModel> Execute()
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
                    catch (Exception ex) when (ex is Win32Exception or AggregateException) { }
                }
            );
            return cb.DistinctBy(x => x.MainModule!.FileName)
                .Select(x => new ProcessModel(x)
                {
                    Bitmap = GetBitmapStreamFromPath(x.MainModule!.FileName),
                });
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
}
