using System.Collections.Generic;
using SharpHook.Native;

namespace YMouseButtonControl.KeyboardAndMouse.Services;

public static class MouseButtonCodeMappings
{
    public static readonly Dictionary<string, MouseButton> MouseButtonCodes = new()
    {
        {"lmb", MouseButton.Button1},
        {"rmb", MouseButton.Button2},
        {"mmb", MouseButton.Button3},
        {"mb4", MouseButton.Button4},
        {"mb5", MouseButton.Button5},
    };
}