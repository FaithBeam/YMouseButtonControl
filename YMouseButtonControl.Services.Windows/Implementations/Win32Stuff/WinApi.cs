using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace YMouseButtonControl.Services.Windows.Implementations.Win32Stuff;

public class WinApi
{
    public delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);
    
    [DllImport("user32.dll")]
    public static extern bool UnhookWinEvent(IntPtr hWinEventHook);

    [DllImport("user32.dll")]
    public static extern IntPtr SetWinEventHook(WinEvents eventMin, WinEvents eventMax, IntPtr hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, WinEventFlags dwFlags);

    [DllImport("psapi.dll")]
    public static extern uint GetModuleFileNameEx(IntPtr hProcess, IntPtr hModule, [Out] StringBuilder lpBaseName, [In][MarshalAs(UnmanagedType.U4)] int nSize);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern IntPtr OpenProcess(uint processAccess, bool bInheritHandle, uint processId);

    [DllImport("user32.dll")]
    public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam,IntPtr lParam);
    
    private static IntPtr GetClassLongPtr(IntPtr hWnd, int nIndex)
    {
        return IntPtr.Size > 4 ? GetClassLongPtr64(hWnd, nIndex) : new IntPtr(GetClassLongPtr32(hWnd, nIndex));
    }

    [DllImport("user32.dll", EntryPoint = "GetClassLong")]
    private static extern uint GetClassLongPtr32(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll", EntryPoint = "GetClassLongPtr")]
    private static extern IntPtr GetClassLongPtr64(IntPtr hWnd, int nIndex);
    
    [DllImport("user32.dll")]
    private static extern bool GetIconInfo(IntPtr hIcon, out ICONINFO piconinfo);
    
    [DllImport("user32.dll", SetLastError=true)]
    public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

    
    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr GetWindow(IntPtr hWnd, GetWindowType uCmd);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool IsWindowVisible(IntPtr hWnd);
    
    [DllImport("shell32.dll", CharSet=CharSet.Auto)]
    static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbFileInfo, uint uFlags);
    
    [DllImport("user32.dll", SetLastError=true, CharSet=CharSet.Auto)]
    static extern int GetWindowTextLength(IntPtr hWnd);
    
    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

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
    public static extern nint SetWindowsHookEx(HookType hookType, nint lpfn, nint hMod, uint dwThreadId);
    
    public delegate IntPtr HookProc(int code, nint wParam, nint lParam);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr SetWindowsHookEx(HookType hookType, HookProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll")]
    public static extern int GetMessage(out MSG lpMsg, nint hWnd, uint wMsgFilterMin, uint wMsgFilterMax);

    [DllImport("user32.dll")]
    public static extern bool TranslateMessage([In] ref MSG lpMsg);

    [DllImport("user32.dll")]
    public static extern nint DispatchMessage([In] ref MSG lpmsg);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool FreeLibrary(IntPtr hModule);

    public IntPtr GetHandleFromProcessId(string pId)
    {
        return OpenProcess((uint)(ProcessAccessFlags.All), false,
            uint.Parse(pId));
    }

    public bool GetWindowTitleFromHwnd(IntPtr hWnd, out string windowTitle)
    {
        var length = GetWindowTextLength(hWnd);
        var sb = new StringBuilder(length + 1);
        GetWindowText(hWnd, sb, sb.Capacity);
        windowTitle = sb.ToString();
        return sb.Length > 0;
    }
    
    public Icon GetBitmapFromPath(string pathToExe)
    {
        if (pathToExe == "/" || !File.Exists(pathToExe))
        {
            return null;
        }
        return Icon.ExtractAssociatedIcon(pathToExe);
    }
    
    public string GetBitmapPathFromProcessId(string pId)
    {
        if (pId == "0")
        {
            return string.Empty;
        }
        var handle = GetHandleFromProcessId(pId);
        if (handle == IntPtr.Zero)
        {
            return string.Empty;
        }
        var sb = new StringBuilder(1024);
        var result = GetModuleFileNameEx(handle, IntPtr.Zero, sb, sb.Capacity);
        var destination = Path.Join("cache", Path.GetFileName(sb + ".ico"));
        if (File.Exists(destination)) return destination;
        if (!Directory.Exists("cache"))
        {
            Directory.CreateDirectory("cache");
        }
        var bitmap = GetBitmapFromPath(sb.ToString());
        if (bitmap is null)
        {
            return string.Empty;
        }
        var bmp = bitmap.ToBitmap();
        bmp.Save(destination);

        return destination;
    }
    
    public string GetBitmapPathFromProcessId(uint pId)
    {
        return GetBitmapPathFromProcessId(pId.ToString());
    }
}