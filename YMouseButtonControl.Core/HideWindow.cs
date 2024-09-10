using System;
using Avalonia.Controls;

namespace YMouseButtonControl.Core;

public static class HideWindow
{
    public static void Hide(object? sender, EventArgs e) => ((Window)sender!).Hide();
}
