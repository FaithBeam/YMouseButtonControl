using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;

namespace YMouseButtonControl.Avalonia.Implementations;

public static class MainWindowProvider
{
    public static Window GetMainWindow()
    {
        var lifetime = (IClassicDesktopStyleApplicationLifetime)(
            Application.Current?.ApplicationLifetime
            ?? throw new Exception("Error retrieving application lifetime")
        );
        if (lifetime.MainWindow is null)
        {
            throw new Exception("Error retrieving application lifetime");
        }

        return lifetime.MainWindow;
    }
}
