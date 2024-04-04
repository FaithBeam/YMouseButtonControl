using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using Serilog;
using Windows.Win32;
using Windows.Win32.System.Threading;
using YMouseButtonControl.Processes.Interfaces;

namespace YMouseButtonControl.Services.Windows.Implementations;

[SupportedOSPlatform("windows5.1.2600")]
public class CurrentWindowService : ICurrentWindowService
{
    private readonly ILogger _log = Log.Logger.ForContext<CurrentWindowService>();

    public string ForegroundWindow => GetWindowTitleFromHWnd();

    private unsafe string GetWindowTitleFromHWnd()
    {
        var hWnd = PInvoke.GetForegroundWindow();
        if (hWnd.IsNull)
        {
            _log.Information("HWND is null");
            return "";
        }
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
            throw new Win32Exception(Marshal.GetLastWin32Error());
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
}
