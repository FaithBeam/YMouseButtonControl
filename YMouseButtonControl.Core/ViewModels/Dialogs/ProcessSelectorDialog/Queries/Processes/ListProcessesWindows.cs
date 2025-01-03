using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.System.Threading;
using YMouseButtonControl.Core.ViewModels.Dialogs.ProcessSelectorDialog.Queries.Processes.Models;

namespace YMouseButtonControl.Core.ViewModels.Dialogs.ProcessSelectorDialog.Queries.Processes;

[SupportedOSPlatform("windows5.1.2600")]
public static partial class ListProcessesWindows
{
    public sealed partial class Handler : IListProcessesHandler
    {
        public IEnumerable<ProcessModel> Execute()
        {
            // List to hold window information
            var windows = new List<WindowInfo>();

            // Create a GCHandle to hold the list
            var handle = GCHandle.Alloc(windows);

            try
            {
                // Enumerate all top-level windows
                PInvoke.EnumWindows(EnumWindowsCallback, GCHandle.ToIntPtr(handle));
            }
            finally
            {
                // Free the GCHandle
                handle.Free();
            }

            return windows.Select(x =>
            {
                var path =
                    GetPath(x.Hwnd)
                    ?? throw new System.Exception(
                        $"Could not find path for hwnd: {x.Hwnd}, window title: {x.Title}"
                    );
                var moduleName = Path.GetFileName(path);
                var processName = Path.GetFileNameWithoutExtension(moduleName); // this needs to be changed to actually get the process name
                return new ProcessModel(x.Title, moduleName, processName, path);
            });
        }

        // Structure to hold window information
        public struct WindowInfo
        {
            internal HWND Hwnd;
            public string Title;
        }

        private static unsafe string? GetPath(HWND hWnd)
        {
            uint pId;
            var res = PInvoke.GetWindowThreadProcessId(hWnd, &pId);
            if (res == 0)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            var hProc = PInvoke.OpenProcess_SafeHandle(
                PROCESS_ACCESS_RIGHTS.PROCESS_QUERY_LIMITED_INFORMATION,
                false,
                pId
            );
            if (hProc is null || hProc.IsInvalid)
            {
                var lastWin32Err = Marshal.GetLastWin32Error();
                if (lastWin32Err != 5)
                {
                    throw new Win32Exception(lastWin32Err);
                }
                return null;
            }

            fixed (char* pText = new char[1024])
            {
                var lenCopied = PInvoke.GetModuleFileNameEx(hProc, null, pText, 1024);
                if (lenCopied == 0)
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }

                return new string(pText);
            }
        }

        // Callback function to be called for each window
        private static unsafe BOOL EnumWindowsCallback(HWND hwnd, LPARAM lParam)
        {
            fixed (char* titleBuff = new char[1024])
            {
                if (PInvoke.GetWindowText(hwnd, titleBuff, 1024) < 1)
                {
                    return true;
                }
                var title = new string(titleBuff);
                // Check if the window is visible and has a title
                if (!PInvoke.IsWindowVisible(hwnd) || title.Length <= 0)
                {
                    return true;
                }
                // Store the window information
                var windows = (List<WindowInfo>)GCHandle.FromIntPtr(lParam).Target!;
                windows.Add(new WindowInfo { Hwnd = hwnd, Title = title });
            }

            return true;
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
