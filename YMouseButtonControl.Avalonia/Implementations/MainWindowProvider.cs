using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using YMouseButtonControl.Avalonia.Interfaces;

namespace YMouseButtonControl.Avalonia.Implementations;

public static class MainWindowProvider
{
    public static Window GetMainWindow()
    {
        var lifetime = (IClassicDesktopStyleApplicationLifetime)
            Application.Current?.ApplicationLifetime;

        return lifetime?.MainWindow;
    }
}
