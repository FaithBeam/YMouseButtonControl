﻿using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using Microsoft.Extensions.Logging;
using Windows.Win32;
using Windows.Win32.System.Threading;
using YMouseButtonControl.Core.Services.Processes;

namespace YMouseButtonControl.Windows.Services;

[SupportedOSPlatform("windows5.1.2600")]
public partial class CurrentWindowService(ILogger<CurrentWindowService> logger)
    : ICurrentWindowService
{
    public string ForegroundWindow => GetWindowTitleFromHWnd();

    private unsafe string GetWindowTitleFromHWnd()
    {
        var hWnd = PInvoke.GetForegroundWindow();
        if (hWnd.IsNull)
        {
            LogHwndNull(logger);
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
            var lastWin32Err = Marshal.GetLastWin32Error();
            if (lastWin32Err != 5)
            {
                throw new Win32Exception(lastWin32Err);
            }
            LogErrorAccessDenied(logger);
            return "";
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

    [LoggerMessage(
        LogLevel.Information,
        "ERROR_ACCESS_DENIED, likely tried to open an admin process without admin permissions. Try opening YMouseButtonControl as admin."
    )]
    private static partial void LogErrorAccessDenied(ILogger logger);

    [LoggerMessage(LogLevel.Information, "HWND is null")]
    private static partial void LogHwndNull(ILogger logger);
}
