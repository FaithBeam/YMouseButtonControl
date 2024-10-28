using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;

namespace YMouseButtonControl.Views;

public class MainWindowProvider : IMainWindowProvider
{
    public Window GetMainWindow()
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
