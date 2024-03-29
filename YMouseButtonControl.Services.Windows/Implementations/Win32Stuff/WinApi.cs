using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace YMouseButtonControl.Services.Windows.Implementations.Win32Stuff;

public class WinApi
{
    public delegate void WinEventDelegate(
        IntPtr hWinEventHook,
        uint eventType,
        IntPtr hwnd,
        int idObject,
        int idChild,
        uint dwEventThread,
        uint dwmsEventTime
    );

    [DllImport("user32.dll")]
    public static extern IntPtr SetWinEventHook(
        WinEvents eventMin,
        WinEvents eventMax,
        IntPtr hmodWinEventProc,
        WinEventDelegate lpfnWinEventProc,
        uint idProcess,
        uint idThread,
        WinEventFlags dwFlags
    );

    [DllImport("psapi.dll")]
    public static extern uint GetModuleFileNameEx(
        IntPtr hProcess,
        IntPtr hModule,
        [Out] StringBuilder lpBaseName,
        [In] [MarshalAs(UnmanagedType.U4)] int nSize
    );

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern IntPtr OpenProcess(
        uint processAccess,
        bool bInheritHandle,
        uint processId
    );

    [DllImport("user32.dll")]
    public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr GetWindow(IntPtr hWnd, GetWindowType uCmd);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool IsWindowVisible(IntPtr hWnd);

    [DllImport("kernel32", SetLastError = true)]
    public static extern nint LoadLibrary(string lpFileName);

    [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
    public static extern nint GetProcAddress(nint hModule, string procName);

    [return: MarshalAs(UnmanagedType.Bool)]
    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool PostThreadMessage(uint threadId, uint msg, nuint wParam, nint lParam);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool UnhookWindowsHookEx(nint hhk);

    [DllImport("kernel32.dll")]
    public static extern uint GetCurrentThreadId();

    [DllImport("user32.dll", SetLastError = true)]
    public static extern nint SetWindowsHookEx(
        HookType hookType,
        nint lpfn,
        nint hMod,
        uint dwThreadId
    );

    public delegate IntPtr HookProc(int code, nint wParam, nint lParam);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr SetWindowsHookEx(
        HookType hookType,
        HookProc lpfn,
        IntPtr hMod,
        uint dwThreadId
    );

    [DllImport("user32.dll")]
    public static extern int GetMessage(
        out MSG lpMsg,
        nint hWnd,
        uint wMsgFilterMin,
        uint wMsgFilterMax
    );

    [DllImport("user32.dll")]
    public static extern bool TranslateMessage([In] ref MSG lpMsg);

    [DllImport("user32.dll")]
    public static extern nint DispatchMessage([In] ref MSG lpmsg);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool FreeLibrary(IntPtr hModule);

    public string GetBitmapFromPath(string path)
    {
        if (path is null or "/")
        {
            return string.Empty;
        }

        var destination = Path.Join("cache", Path.GetFileName(path + ".ico"));
        if (File.Exists(destination))
        {
            return destination;
        }

        if (!Directory.Exists("cache"))
        {
            Directory.CreateDirectory("cache");
        }

        var icon = Icon.ExtractAssociatedIcon(path);
        if (icon is null)
        {
            return string.Empty;
        }

        var bmp = icon.ToBitmap();
        bmp.Save(destination);

        return destination;
    }
}
