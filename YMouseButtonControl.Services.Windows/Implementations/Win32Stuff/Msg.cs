using System;
using System.Runtime.InteropServices;

namespace YMouseButtonControl.Services.Windows.Implementations.Win32Stuff;

[StructLayout(LayoutKind.Sequential)]

public struct MSG
{
    public IntPtr hwnd;
    public uint message;
    public UIntPtr wParam;
    public IntPtr lParam;
    public int time;
    public POINT pt;
    public int lPrivate;
}
