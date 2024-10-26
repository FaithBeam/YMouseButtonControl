using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Runtime.Versioning;
using SharpHook.Native;

namespace YMouseButtonControl.Core.Services.KeyboardAndMouse;

public interface IMouseButtonMappingService
{
    FrozenDictionary<string, MouseButton> MouseButtons { get; }
}

public class MouseButtonMappingService : IMouseButtonMappingService
{
    public MouseButtonMappingService()
    {
        if (OperatingSystem.IsLinux())
        {
            MouseButtons = LinuxMouseButtons.ToFrozenDictionary();
        }
        else if (OperatingSystem.IsMacOS() || OperatingSystem.IsWindows())
        {
            MouseButtons = NonLinuxMouseButtons.ToFrozenDictionary();
        }
        else
        {
            throw new PlatformNotSupportedException();
        }
    }

    public FrozenDictionary<string, MouseButton> MouseButtons { get; }

    // rmb and mmb are flipped in X11 Linux
    [SupportedOSPlatform("linux")]
    private static readonly Dictionary<string, MouseButton> LinuxMouseButtons =
        new()
        {
            { "lmb", MouseButton.Button1 },
            { "rmb", MouseButton.Button3 },
            { "mmb", MouseButton.Button2 },
            { "mb4", MouseButton.Button4 },
            { "mb5", MouseButton.Button5 },
        };

    [SupportedOSPlatform("windows")]
    [SupportedOSPlatform("osx")]
    private static readonly Dictionary<string, MouseButton> NonLinuxMouseButtons =
        new()
        {
            { "lmb", MouseButton.Button1 },
            { "rmb", MouseButton.Button2 },
            { "mmb", MouseButton.Button3 },
            { "mb4", MouseButton.Button4 },
            { "mb5", MouseButton.Button5 },
        };
}
