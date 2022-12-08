using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace YMouseButtonControl.Services.Windows.Implementations;

public class WinApi
{
    [DllImport("psapi.dll")]
    static extern uint GetModuleFileNameEx(IntPtr hProcess, IntPtr hModule, [Out] StringBuilder lpBaseName, [In] [MarshalAs(UnmanagedType.U4)] int nSize);
    
    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr OpenProcess(uint processAccess, bool bInheritHandle, uint processId);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, ref EnumWindowData lParam);

    internal delegate bool EnumWindowsProc(IntPtr hWnd, ref EnumWindowData lParam);
    
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
    static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

    private EnumWindowsProc _enumWindowsProc = EnumWindowsProcCallback;
    
    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr GetWindow(IntPtr hWnd, GetWindowType uCmd);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool IsWindowVisible(IntPtr hWnd);
    
    [DllImport("shell32.dll", CharSet=CharSet.Auto)]
    static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbFileInfo, uint uFlags);
    
    [DllImport("user32.dll", SetLastError=true, CharSet=CharSet.Auto)]
    static extern int GetWindowTextLength(IntPtr hWnd);
    
    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
    
    private static bool EnumWindowsProcCallback(IntPtr hWnd, ref EnumWindowData lParam)
    {
        GetWindowThreadProcessId(hWnd, out var processId);
        if (lParam.ProcessId != processId || !IsMainWindow(hWnd))
        {
            return true;
        }

        lParam.HWnd = hWnd;
        return false;
    }

    private static bool IsMainWindow(IntPtr hWnd)
    {
        var getWinRes = GetWindow(hWnd, GetWindowType.GW_OWNER);
        var isWinVis = IsWindowVisible(hWnd);
        return getWinRes == IntPtr.Zero && isWinVis;
    }
    
    public IntPtr GetHWndFromProcessId(string pId)
    {
        return OpenProcess((uint)(ProcessAccessFlags.All), false,
            uint.Parse(pId));
    }

    internal struct EnumWindowData
    {
        public uint ProcessId;
        public IntPtr HWnd;
    }
    
    public IntPtr GetHWndFromProcessId(int pId)
    {
        var data = new EnumWindowData
        {
            ProcessId = (uint)pId
        };
        EnumWindows(EnumWindowsProcCallback, ref data);
        return data.HWnd;
    }
    
    public IntPtr GetHWndFromProcessId(uint pId)
    {
        var data = new EnumWindowData
        {
            ProcessId = pId
        };
        EnumWindows(EnumWindowsProcCallback, ref data);
        return data.HWnd;
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
        var handle = GetHWndFromProcessId(pId);
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
        var bmp = bitmap.ToBitmap();
        bmp.Save(destination);

        return destination;
    }
    
    public string GetBitmapPathFromProcessId(uint pId)
    {
        return GetBitmapPathFromProcessId(pId.ToString());
    }
}