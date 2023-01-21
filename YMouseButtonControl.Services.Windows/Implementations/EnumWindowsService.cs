using System;
using System.Runtime.InteropServices;
using YMouseButtonControl.Services.Windows.Implementations.Win32Stuff;

namespace YMouseButtonControl.Services.Windows.Implementations;

public static class EnumWindowsService
{
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, ref EnumWindowData lParam);

    delegate bool EnumWindowsProc(IntPtr hWnd, ref EnumWindowData lParam);

    static EnumWindowsProc _enumWindowsProc = EnumWindowsProcCallback;

    private static bool EnumWindowsProcCallback(IntPtr hWnd, ref EnumWindowData lParam)
    {
        WinApi.GetWindowThreadProcessId(hWnd, out var processId);
        if (lParam.ProcessId != processId || !IsMainWindow(hWnd))
        {
            return true;
        }

        lParam.HWnd = hWnd;
        return false;
    }

    private static bool IsMainWindow(IntPtr hWnd)
    {
        var getWinRes = WinApi.GetWindow(hWnd, GetWindowType.GW_OWNER);
        var isWinVis = WinApi.IsWindowVisible(hWnd);
        return getWinRes == IntPtr.Zero && isWinVis;
    }

    internal struct EnumWindowData
    {
        public uint ProcessId;
        public IntPtr HWnd;
    }

    public static uint GetTidAssociatedWithWindowFromPid(uint pId)
    {
        var hWnd = GetHWndFromThreadId(pId);
        var tid = WinApi.GetWindowThreadProcessId(hWnd, out var proc);
        return tid;
    }

    public static IntPtr GetHWndFromThreadId(int pId)
    {
        var data = new EnumWindowData
        {
            ProcessId = (uint)pId
        };
        EnumWindows(EnumWindowsProcCallback, ref data);
        return data.HWnd;
    }

    public static IntPtr GetHWndFromThreadId(uint pId)
    {
        var data = new EnumWindowData
        {
            ProcessId = pId
        };
        EnumWindows(EnumWindowsProcCallback, ref data);
        return data.HWnd;
    }
}
