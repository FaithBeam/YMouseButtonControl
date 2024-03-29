using System;
using System.Runtime.InteropServices;

namespace YMouseButtonControl.Services.Windows.Implementations.Win32Stuff;

[StructLayout(LayoutKind.Sequential)]
public struct MOUSEHOOKSTRUCT
{
    public POINT pt;
    public IntPtr hwnd;
    public uint wHitTestCode;
    public IntPtr dwExtraInfo;
}

[StructLayout(LayoutKind.Sequential)]
public struct MOUSEHOOKSTRUCTEX
{
    public MOUSEHOOKSTRUCT mouseHookStruct;
    public uint mouseData;
}
