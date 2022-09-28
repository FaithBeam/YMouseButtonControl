using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Splat;
using YMouseButtonControl.Core.ViewModels;

namespace YMouseButtonControl.UI;

public class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            DataContext = GetRequiredService<MainWindowViewModel>();
            desktop.MainWindow = new Views.MainWindow()
            {
                DataContext = DataContext
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private static T GetRequiredService<T>() => Locator.Current.GetRequiredService<T>();
}

