using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using Windows.Win32;
using Windows.Win32.Foundation;
using YMouseButtonControl.Core.ViewModels.Dialogs.FindWindowDialog.Queries.WindowUnderCursor.Models;
using static Windows.Win32.System.Threading.PROCESS_ACCESS_RIGHTS;

namespace YMouseButtonControl.Core.ViewModels.Dialogs.FindWindowDialog.Queries.WindowUnderCursor;

public static class WindowUnderCursorWindows
{
    [SupportedOSPlatform("windows5.1.2600")]
    public sealed class Handler : IWindowUnderCursorHandler
    {
        private HWND? _hwnd;
        private string? _path;

        public unsafe Response? Execute(Query q)
        {
            var p = new Point { X = q.X, Y = q.Y };

            var hwnd = PInvoke.WindowFromPoint(p);
            if (hwnd.IsNull)
            {
                return null;
            }

            if (_hwnd is null)
            {
                _hwnd = hwnd;
            }
            else if (_hwnd.Value == hwnd.Value && !string.IsNullOrWhiteSpace(_path))
            {
                return new Response(_path);
            }

            uint pId;
            var res = PInvoke.GetWindowThreadProcessId(hwnd, &pId);
            if (res == 0)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            var hProc = PInvoke.OpenProcess_SafeHandle(
                PROCESS_QUERY_LIMITED_INFORMATION,
                false,
                pId
            );
            if (hProc.IsInvalid)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            fixed (char* pText = new char[1024])
            {
                var lenCopied = PInvoke.GetProcessImageFileName(hProc, pText, 1024);
                if (lenCopied == 0)
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
                _path = new string(pText);
                return new Response(_path);
            }
        }
    }
}
