using System.Runtime.InteropServices;
using YMouseButtonControl.Core.Services.Processes;

namespace YMouseButtonControl.Linux.Services;

public class CurrentWindowServiceX11 : ICurrentWindowService
{
    public string ForegroundWindow => GetForegroundWindow();

    private static string GetForegroundWindow()
    {
        var display = X11.XOpenDisplay(nint.Zero);
        if (display == nint.Zero)
        {
            throw new Exception("Error opening display");
        }

        try
        {
            var pid = GetForegroundWindowPid(display);
            if (pid is null)
            {
                return "";
            }

            return GetPathFromPid(pid) ?? "";
        }
        finally
        {
            X11.XCloseDisplay(display);
        }
    }

    private static string? GetPathFromPid(int? pid)
    {
        var fi = new FileInfo($"/proc/{pid}/exe");
        return fi.LinkTarget;
    }

    private static unsafe int? GetForegroundWindowPid(nint display)
    {
        var root = X11.XDefaultRootWindow(display);
        var prop = X11.XInternAtom(display, Marshal.StringToHGlobalAnsi("_NET_ACTIVE_WINDOW"), 0);
        var pidProp = X11.XInternAtom(display, Marshal.StringToHGlobalAnsi("_NET_WM_PID"), 1);

        if (X11.XGetWindowProperty(display, root, prop, 0, sizeof(ulong), 0, 0,
                out _, out _, out _,
                out _, out var outProp) != 0 || outProp == nint.Zero)
        {
            return null;
        }

        var activeWindow = *(nint*)outProp;
        X11.XFree(outProp);

        if (X11.XGetWindowProperty(display, activeWindow, pidProp, 0, sizeof(int), 0, 0, out _, out _, out _, out _,
                out var prop2) != 0 || prop2 == nint.Zero)
        {
            return null;
        }

        var pid = *(int*)prop2;
        X11.XFree(prop2);
        return pid;
    }
}

internal static partial class X11
{
    [LibraryImport("libX11.so")]
    internal static partial int XFree(nint data);

    [LibraryImport("libX11.so")]
    internal static partial nint XOpenDisplay(nint display);

    [LibraryImport("libX11.so")]
    internal static partial void XCloseDisplay(nint display);

    [LibraryImport("libX11.so")]
    internal static partial nint XDefaultRootWindow(nint display);

    [LibraryImport("libX11.so")]
    internal static partial nint XInternAtom(nint display, nint atomName, int onlyIfExists);

    [LibraryImport("libX11.so")]
    internal static partial int XGetWindowProperty(
        IntPtr display,
        IntPtr window,
        IntPtr property,
        long longOffset,
        long longLength,
        int delete,
        ulong reqType,
        out ulong actualTypeReturn,
        out int actualFormatReturn,
        out ulong nItemsReturn,
        out ulong bytesAfterReturn,
        out IntPtr propReturn);
}