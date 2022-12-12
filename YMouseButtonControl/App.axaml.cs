using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Splat;
using YMouseButtonControl.DependencyInjection;
using YMouseButtonControl.ViewModels.Implementations;
using YMouseButtonControl.ViewModels.Interfaces;
using YMouseButtonControl.Views;

namespace YMouseButtonControl;

public class App : Application
{
    public App()
    {
        DataContext = new AppViewModel();
    }
    
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
            // Prevent the application from exiting and hide the window when the user presses the X button
            desktop.MainWindow.Closing += (s, e) =>
            {
                ((Window)s!).Hide();
                e.Cancel = true;
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private static T GetRequiredService<T>() => Locator.Current.GetRequiredService<T>();
}
