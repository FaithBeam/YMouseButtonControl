using Avalonia;
using Avalonia.ReactiveUI;
using Splat;
using YMouseButtonControl.UI;

namespace YMouseButtonControl;

internal static class Program
{
    public static void Main(string[] args)
    {
        Bootstrapper.Register(Locator.CurrentMutable, Locator.Current, dataAccessConfig);
        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
    }

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .LogToTrace()
            .UseReactiveUI();
}